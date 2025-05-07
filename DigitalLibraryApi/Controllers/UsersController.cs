using DigitalLibraryConsole.Models;
using DigitalLibraryConsole.Service;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly LibraryService _libraryService;

        public UsersController(LibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        //Get all users
        // GET: api/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _libraryService.GetAllUsers();
            return Ok(users);
        }

        //Get user by id
        // GET: api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _libraryService.GetAllUsers().FirstOrDefault(u => u.Id == id);
            return user == null ? NotFound() : Ok(user);
        }

        //Get user by name
        //Get: api/users/name/{name}
        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)
        {
            var user = _libraryService.GetAllUsers().FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return user == null ? NotFound() : Ok(user);
        }

        //Get user by surname
        //Get: api/users/surname/{surname}
        [HttpGet("surname/{surname}")]
        public IActionResult GetBySurname(string surname)
        {
            var user = _libraryService.GetAllUsers().FirstOrDefault(u => u.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
            return user == null ? NotFound() : Ok(user);
        }

        //Get user by phone number
        //Get: api/users/phone/{phoneNumber}
        [HttpGet("phone/{phoneNumber}")]
        public IActionResult GetByPhoneNumber(string phoneNumber)
        {
            var user = _libraryService.GetAllUsers().FirstOrDefault(u => u.PhoneNumber.Equals(phoneNumber, StringComparison.OrdinalIgnoreCase));
            return user == null ? NotFound() : Ok(user);
        }

        //Post user
        // POST: api/users
        [HttpPost]
        public IActionResult Create(User user)
        {
            _libraryService.RegisterUser(user);
            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
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
