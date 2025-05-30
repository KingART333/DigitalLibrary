﻿using DigitalLibraryConsole.Models;
using DigitalLibraryConsole.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryService _libraryService;

        public BooksController(LibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        //Get all books
        // GET: api/books
        [HttpGet]
        public IActionResult GetAllBooks(int pageNumber = 1, int pageSize = 10)
        {
            var books = _libraryService.GetAvailableBooks(pageNumber, pageSize);
            return Ok(books);
        }

        //Get book by id
        // POST: api/books
        [HttpPost]
        public IActionResult Create(Book book)
        {
            _libraryService.AddBook(book);
            return CreatedAtAction(nameof(GetAllBooks), new { id = book.Id }, book);
        }

        //Get book by Title
        //Get: api/books/title/{title}
        [HttpGet("title/{title}")]
        public IActionResult GetByTitle(string title)
        {
            var book = _libraryService.GetBookByTitle(title);
            return book == null ? NotFound() : Ok(book);
        }

        //Get book by Author
        //Get: api/books/author/{author}
        [HttpGet("author/{author}")]
        public IActionResult GetByAuthor(string author, int pageNumber = 1, int pageSize = 10)
        {
            var books = _libraryService.GetBooksByAuthor(author, pageNumber, pageSize);
            return books == null || books.Count == 0 ? NotFound() : Ok(books);
        }

        //Get book by ISBN
        //Get: api/books/isbn/{isbn}
        [HttpGet("isbn/{isbn}")]
        public IActionResult GetByISBN(string isbn)
        {
            var book = _libraryService.GetBookByISBN(isbn);
            return book == null ? NotFound() : Ok(book);
        }

        //Get book by id
        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _libraryService.DeleteBook(id);
            return NoContent();
        }
    }
}
