using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibraryConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibraryConsole.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LendingRecord> LendingRecords { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "library.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }



        public static void Seed(LibraryContext context)
        {
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book { Title = "1984", Author = "George Orwell", ISBN = "1234567890", NumberOfAvailableCopies = 5 },
                    new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "0987654321", NumberOfAvailableCopies = 3 }
                );
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Name = "John", Surname = "Doe", PhoneNumber = "123-456-7890" },
                    new User { Name = "Jane", Surname = "Smith", PhoneNumber = "098-765-4321" }
                );
            }

            context.SaveChanges();
        }
    }
}
