using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalLibraryConsole.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int NumberOfAvailableCopies { get; set; }

        public Book() { }
        public Book(string title, string author, string isbn, int numberOfAvailableCopies)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            NumberOfAvailableCopies = numberOfAvailableCopies;
        }
    }
}
