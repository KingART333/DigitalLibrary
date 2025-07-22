using DigitalLibraryConsole.Data;
using DigitalLibraryConsole.Models;
using DigitalLibraryConsole.Service;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
        .UseSqlite($"Data Source={Path.Combine(AppContext.BaseDirectory, "library.db")}")
        .Options;

        using var context = new LibraryContext(options);

        LibraryContext.Seed(context);

        var service = new LibraryService(context);

        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("Library Management System");
            Console.WriteLine("1. Register User");
            Console.WriteLine("2. Delete User");
            Console.WriteLine("3. Add Book");
            Console.WriteLine("4. Borrow Book");
            Console.WriteLine("5. Return Book");
            Console.WriteLine("6. View All Users");
            Console.WriteLine("7. View Available Books");
            Console.WriteLine("8. View Overdue Books");
            Console.WriteLine("9. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    RegisterUser(service);
                    break;
                case "2":
                    if (TryReadInt("User ID to delete: ", out int deleteUserId))
                    {
                        service.DeleteUser(deleteUserId);
                        Console.WriteLine("User deleted successfully!");
                    }
                    Console.ReadKey();
                    break;
                case "3":
                    AddBook(service);
                    break;
                case "4":
                    BorrowBook(service);
                    break;
                case "5":
                    ReturnBook(service);
                    break;
                case "6":
                    var users = service.GetAllUsers();
                    Console.WriteLine("All Users:");
                    foreach (var user in users)
                    {
                        Console.WriteLine($"{user.Id}. {user.Name} {user.Surname} - {user.PhoneNumber}");
                    }
                    Console.ReadKey();
                    break;
                case "7":
                    ViewAvailableBooks(service);
                    break;
                case "8":
                    ViewOverdueBooks(service);
                    break;
                case "9":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void RegisterUser(LibraryService service)
    {
        Console.WriteLine("Register User");
        Console.WriteLine("Age: ");
        Console.Write("Age: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("Invalid input for age.");
            Console.ReadKey();
            return;
        }
        Console.Write("Name: ");
        var name = Console.ReadLine();
        Console.Write("Surname: ");
        var surname = Console.ReadLine();
        Console.Write("Phone Number: ");
        var phoneNumber = Console.ReadLine();


        var user = new User(age ,name, surname, phoneNumber);
        service.RegisterUser(user);
        Console.WriteLine("User registered successfully!");
        Console.ReadKey();
    }

    static void AddBook(LibraryService service)
    {
        Console.WriteLine("Add Book");
        Console.Write("Title: ");
        var title = Console.ReadLine();
        Console.Write("Author: ");
        var author = Console.ReadLine();
        Console.Write("ISBN: ");
        var isbn = Console.ReadLine();

        if (!TryReadInt("Number of Copies: ", out int numberOfCopies)) return;

        var book = new Book(title, author, isbn, numberOfCopies);
        service.AddBook(book);
        Console.WriteLine("Book added successfully!");
        Console.ReadKey();
    }

    static void BorrowBook(LibraryService service)
    {
        Console.WriteLine("Borrow Book");

        if (!TryReadInt("User ID: ", out int userId)) return;
        if (!TryReadInt("Book ID: ", out int bookId)) return;

        if (service.BorrowBook(userId, bookId))
            Console.WriteLine("Book borrowed successfully!");
        else
            Console.WriteLine("Failed to borrow the book.");
        Console.ReadKey();
    }

    static void ReturnBook(LibraryService service)
    {
        Console.WriteLine("Return Book");

        if (!TryReadInt("Lending Record ID: ", out int recordId)) return;

        if (service.ReturnBook(recordId))
            Console.WriteLine("Book returned successfully!");
        else
            Console.WriteLine("Failed to return the book.");
        Console.ReadKey();
    }

    static void ViewAvailableBooks(LibraryService service)
    {
        var availableBooks = service.GetAvailableBooks();
        Console.WriteLine("Available Books:");
        foreach (var book in availableBooks)
        {
            Console.WriteLine($"{book.Id}. {book.Title} by {book.Author} ({book.NumberOfAvailableCopies} copies available)");
        }
        Console.ReadKey();
    }

    static void ViewOverdueBooks(LibraryService service)
    {
        var overdueRecords = service.GetOverdueRecords();
        Console.WriteLine("Overdue Books:");
        foreach (var record in overdueRecords)
        {
            Console.WriteLine($"{record.Book.Title} by {record.Book.Author} - Due Date: {record.DueDate.ToShortDateString()}");
        }
        Console.ReadKey();
    }

    static bool TryReadInt(string prompt, out int result)
    {
        Console.Write(prompt);
        if (!int.TryParse(Console.ReadLine(), out result))
        {
            Console.WriteLine("Invalid number input.");
            Console.ReadKey();
            return false;
        }
        return true;
    }
}
