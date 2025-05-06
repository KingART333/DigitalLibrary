using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalLibraryConsole.Models
{
    public class LendingRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }

        public LendingRecord() { }
        public LendingRecord(int userId, int bookId, DateTime borrowDate, DateTime dueDate)
        {
            UserId = userId;
            BookId = bookId;
            BorrowDate = borrowDate;
            DueDate = dueDate;
        }
    }
}
