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

        //Get all lending records
        // GET: api/lendingrecords
        [HttpGet]
        public IActionResult GetAllLendingRecords()
        {
            var records = _libraryService.GetAllLendingRecords();
            return Ok(records);
        }

        //Get lending record by id
        // GET: api/lendingrecords/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var record = _libraryService.GetAllLendingRecords().FirstOrDefault(r => r.Id == id);
            return record == null ? NotFound() : Ok(record);
        }

        //Get lending record by user id
        // GET: api/lendingrecords/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var records = _libraryService.GetAllLendingRecords()
                .Where(r => r.UserId == userId).ToList();
            return records.Count == 0 ? NotFound() : Ok(records);
        }

        //Get lending record by book id
        // GET: api/lendingrecords/book/{bookId}
        [HttpGet("book/{bookId}")]
        public IActionResult GetByBookId(int bookId)
        {
            var records = _libraryService.GetAllLendingRecords()
                .Where(r => r.BookId == bookId).ToList();
            return records.Count == 0 ? NotFound() : Ok(records);
        }

        //Get all overdue lending record by id
        // GET: api/lendingrecords/overdue
        [HttpGet("overdue")]
        public IActionResult GetOverdue()
        {
            var records = _libraryService.GetOverdueRecords();
            return Ok(records);
        }

        //Get all overdue lending record by user id
        // GET: api/lendingrecords/overdue/user/{userId}
        [HttpGet("overdue/user/{userId}")]
        public IActionResult GetOverdueByUserId(int userId)
        {
            var records = _libraryService.GetOverdueRecords()
                .Where(r => r.UserId == userId).ToList();
            return records.Count == 0 ? NotFound() : Ok(records);
        }

        //Get all overdue lending record by book id
        // GET: api/lendingrecords/overdue/book/{bookId}
        [HttpGet("overdue/book/{bookId}")]
        public IActionResult GetOverdueByBookId(int bookId)
        {
            var records = _libraryService.GetOverdueRecords()
                .Where(r => r.BookId == bookId).ToList();
            return records.Count == 0 ? NotFound() : Ok(records);
        }

        //Getting book
        // POST: api/lendingrecords
        [HttpPost]
        public IActionResult BorrowBook([FromBody] CreateLendingRecordDto dto)
        {
            var success = _libraryService.BorrowBook(dto.UserId, dto.BookId);

            if (!success)
                return BadRequest("Borrow failed. Check user or book availability.");

            // Optionally: fetch the newly created record if needed (e.g., from LendingRecords with latest borrowDate)
            return Ok("Book borrowed successfully.");
        }

        //Returning book
        // PUT: api/lendingrecords/return/{recordId}
        [HttpPut("return/{recordId}")]
        public IActionResult ReturnBook(int recordId)
        {
            var success = _libraryService.ReturnBook(recordId);

            if (!success)
                return BadRequest("Return failed. Invalid record or already returned.");

            var updated = _libraryService.GetAllLendingRecords()
                .FirstOrDefault(r => r.Id == recordId);

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

        //Delete lending record
        // DELETE: api/lendingrecords/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteLendingRecord(int id)
        {
            _libraryService.DeleteLendingRecord(id);
            return NoContent();
        }
    }
}
