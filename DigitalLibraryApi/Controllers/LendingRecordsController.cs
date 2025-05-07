using DigitalLibraryApi.DTOs;
using DigitalLibraryConsole.Models;
using DigitalLibraryConsole.Service;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LendingRecordsController : ControllerBase
    {
        private readonly LibraryService _libraryService;

        public LendingRecordsController(LibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // Get all lending records
        // GET: api/lendingrecords
        [HttpGet]
        public IActionResult GetAllLendingRecords(int pageNumber = 1, int pageSize = 10)
        {
            var records = _libraryService.GetAllLendingRecords(pageNumber, pageSize);
            return Ok(records);
        }

        // Get lending record by id
        // GET: api/lendingrecords/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var record = _libraryService.GetLendingRecordById(id);
            return record == null ? NotFound() : Ok(record);
        }

        // Get lending records by user id
        // GET: api/lendingrecords/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetByUserId(int userId, int pageNumber = 1, int pageSize = 10)
        {
            var records = _libraryService.GetLendingRecordsByUser(userId, pageNumber, pageSize);
            return records.Count == 0 ? NotFound() : Ok(records);
        }

        // Get lending records by book id
        // GET: api/lendingrecords/book/{bookId}
        [HttpGet("book/{bookId}")]
        public IActionResult GetByBookId(int bookId, int pageNumber = 1, int pageSize = 10)
        {
            var records = _libraryService.GetLendingRecordsByBook(bookId, pageNumber, pageSize);
            return records.Count == 0 ? NotFound() : Ok(records);
        }

        // Get all overdue lending records (paginated)
        // GET: api/lendingrecords/overdue
        [HttpGet("overdue")]
        public IActionResult GetOverdue(int pageNumber = 1, int pageSize = 10)
        {
            var records = _libraryService.GetOverdueRecords(pageNumber, pageSize);
            return Ok(records);
        }

        // Get overdue lending records by user id (paginated)
        // GET: api/lendingrecords/overdue/user/{userId}
        [HttpGet("overdue/user/{userId}")]
        public IActionResult GetOverdueByUserId(int userId, int pageNumber = 1, int pageSize = 10)
        {
            var records = _libraryService.GetOverdueRecordsByUser(userId, pageNumber, pageSize);
            return records.Count == 0 ? NotFound() : Ok(records);
        }

        // Get overdue lending records by book id (paginated)
        // GET: api/lendingrecords/overdue/book/{bookId}
        [HttpGet("overdue/book/{bookId}")]
        public IActionResult GetOverdueByBookId(int bookId, int pageNumber = 1, int pageSize = 10)
        {
            var records = _libraryService.GetOverdueRecordsByBook(bookId, pageNumber, pageSize);
            return records.Count == 0 ? NotFound() : Ok(records);
        }

        // Borrow book
        // POST: api/lendingrecords/borrow
        [HttpPost]
        public IActionResult BorrowBook([FromBody] CreateLendingRecordDto dto)
        {
            var success = _libraryService.BorrowBook(dto.UserId, dto.BookId);

            if (!success)
                return BadRequest("Borrow failed. Check user or book availability.");

            return Ok("Book borrowed successfully.");
        }

        // Return book
        // PUT: api/lendingrecords/return/{recordId}
        [HttpPut("return/{recordId}")]
        public IActionResult ReturnBook(int recordId)
        {
            var success = _libraryService.ReturnBook(recordId);

            if (!success)
                return BadRequest("Return failed. Invalid record or already returned.");

            var updated = _libraryService.GetLendingRecordById(recordId);
            if (updated == null)
                return NotFound();

            var dto = new LendingRecordDto
            {
                Id = updated.Id,
                BookTitle = updated.Book.Title,
                UserFullName = $"{updated.User.Name} {updated.User.Surname}",
                BorrowDate = updated.BorrowDate,
                DueDate = updated.DueDate,
                ReturnDate = updated.ReturnDate
            };

            return Ok(dto);
        }

        // Delete lending record
        // DELETE: api/lendingrecords/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteLendingRecord(int id)
        {
            _libraryService.DeleteLendingRecord(id);
            return NoContent();
        }
    }
}
