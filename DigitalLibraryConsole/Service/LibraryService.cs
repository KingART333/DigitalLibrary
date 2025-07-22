using DigitalLibraryConsole.Data;
using DigitalLibraryConsole.Interfaces;
using DigitalLibraryConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibraryConsole.Service
{
    public class LibraryService : ILibraryService
    {
        private readonly LibraryContext _context;

        public LibraryService(LibraryContext context)
        {
            _context = context;
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void DeleteBook(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public void RegisterUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User? GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserByName(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Name.ToLower() == name.ToLower());
        }

        public User? GetUserBySurname(string surname)
        {
            return _context.Users.FirstOrDefault(u => u.Surname.ToLower() == surname.ToLower());
        }

        public User? GetUserByPhoneNumber(string phoneNumber)
        {
            return _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        }


        public bool BorrowBook(int userId, int bookId)
        {
            try
            {
                var book = _context.Books.Find(bookId);
                if (book == null || book.NumberOfAvailableCopies <= 0)
                {
                    Console.WriteLine("Book not available for borrowing.");
                    return false; // Book not available
                }

                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return false; // User not found
                }

                var record = new LendingRecord(userId, bookId, DateTime.Now, DateTime.Now.AddDays(14));
                _context.LendingRecords.Add(record);
                book.NumberOfAvailableCopies--;
                _context.SaveChanges();

                return true; // Book borrowed successfully
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Error occurred
            }
        }

        public bool ReturnBook(int recordId)
        {
            var record = _context.LendingRecords.Find(recordId);
            if (record == null || record.ReturnDate != null)
            {
                Console.WriteLine("Invalid record.");
                return false;
            }

            record.ReturnDate = DateTime.Now;

            var book = _context.Books.Find(record.BookId);
            if (book == null)
                return false;
            _context.SaveChanges();
            return true;
        }


        public List<Book> GetAvailableBooks(int pageNumber = 1, int pageSize = 10)
        {
            return _context.Books
                .Where(b => b.NumberOfAvailableCopies > 0)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<User> GetAllUsers(int pageNumber = 1, int pageSize = 10)
        {
            return _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<LendingRecord> GetAllLendingRecords(int pageNumber = 1, int pageSize = 10)
        {
            return _context.LendingRecords
                .Include(r => r.User)
                .Include(r => r.Book)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<LendingRecord> GetOverdueRecords(int pageNumber = 1, int pageSize = 10)
        {
            return _context.LendingRecords
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.ReturnDate == null && r.DueDate < DateTime.Now)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<LendingRecord> GetOverdueRecordsByUser(int userId, int pageNumber, int pageSize)
        {
            return _context.LendingRecords
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.UserId == userId && r.ReturnDate == null && r.DueDate < DateTime.Now)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<LendingRecord> GetOverdueRecordsByBook(int bookId, int pageNumber, int pageSize)
        {
            return _context.LendingRecords
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.BookId == bookId && r.ReturnDate == null && r.DueDate < DateTime.Now)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public void DeleteLendingRecord(int id)
        {
            var record = _context.LendingRecords.Find(id);
            if (record != null)
            {
                var book = _context.Books.Find(record.BookId);
                if (book != null && record.ReturnDate == null)
                {
                    book.NumberOfAvailableCopies++;
                }

                _context.LendingRecords.Remove(record);
                _context.SaveChanges();
            }
        }

        public List<LendingRecord> GetLendingRecordsByUser(int userId, int pageNumber, int pageSize)
        {
            return _context.LendingRecords
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<LendingRecord> GetLendingRecordsByBook(int bookId, int pageNumber, int pageSize)
        {
            return _context.LendingRecords
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.BookId == bookId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public LendingRecord? GetLendingRecordById(int id)
        {
            return _context.LendingRecords
                .Include(r => r.User)
                .Include(r => r.Book)
                .FirstOrDefault(r => r.Id == id);
        }

        public Book? GetBookByTitle(string title)
        {
            return _context.Books
                .FirstOrDefault(b => b.Title.ToLower() == title.ToLower());
        }

        public List<Book> GetBooksByAuthor(string author, int pageNumber = 1, int pageSize = 10)
        {
            return _context.Books
                .Where(b => EF.Functions.Like(b.Author, author))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Book? GetBookByISBN(string isbn)
        {
            return _context.Books
                .FirstOrDefault(b => b.ISBN.ToLower() == isbn.ToLower());
        }


    }
}
