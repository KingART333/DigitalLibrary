using DigitalLibraryConsole.Interfaces;
using DigitalLibraryConsole.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public UsersController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // Get all users
        // GET: api/users
        [HttpGet]
        public IActionResult GetAllUsers(int pageNumber = 1, int pageSize = 10)
        {
            var users = _libraryService.GetAllUsers(pageNumber, pageSize);
            return Ok(users);
        }

        // Get user by id
        // GET: api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _libraryService.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }

        // Get user by name
        // GET: api/users/name/{name}
        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)
        {
            var user = _libraryService.GetUserByName(name);
            return user == null ? NotFound() : Ok(user);
        }

        //Get user by surname
        // GET: api/users/surname/{surname}
        [HttpGet("surname/{surname}")]
        public IActionResult GetBySurname(string surname)
        {
            var user = _libraryService.GetUserBySurname(surname);
            return user == null ? NotFound() : Ok(user);
        }

        // Get user by PhoneNumber
        // GET: api/users/phone/{phoneNumber}
        [HttpGet("phone/{phoneNumber}")]
        public IActionResult GetByPhoneNumber(string phoneNumber)
        {
            var user = _libraryService.GetUserByPhoneNumber(phoneNumber);
            return user == null ? NotFound() : Ok(user);
        }

        //Post user
        // POST: api/users
        [HttpPost]
        public IActionResult Create(User user)
        {
            _libraryService.RegisterUser(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        //Delete user
        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _libraryService.DeleteUser(id);
            return NoContent();
        }
    }
}
