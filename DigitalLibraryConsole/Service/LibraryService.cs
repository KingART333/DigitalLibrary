using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibraryConsole.Data;
using DigitalLibraryConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibraryConsole.Service
{
    public class LibraryService
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


        public List<Book> GetAvailableBooks()
        {
            return _context.Books.Where(b => b.NumberOfAvailableCopies > 0).ToList();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public List<LendingRecord> GetAllLendingRecords()
        {
            return _context.LendingRecords
                .Include(r => r.User)
                .Include(r => r.Book)
                .ToList();
        }

        public List<LendingRecord> GetOverdueRecords()
        {
            return _context.LendingRecords
                .Where(r => r.ReturnDate == null && r.DueDate < DateTime.Now)
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

    }
}
