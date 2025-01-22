using LibraryManagementAPI.Services;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _libraryService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while retrieving users.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _libraryService.GetUserByIdAsync(id);
                if (user == null) return NotFound(new { Message = $"User with ID {id} not found." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while retrieving the user.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                await _libraryService.AddUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while adding the user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            try
            {
                if (id != updatedUser.Id)
                    return BadRequest(new { Message = "User ID mismatch." });

                await _libraryService.UpdateUserAsync(updatedUser);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _libraryService.GetUserByIdAsync(id);
                if (user == null) return NotFound(new { Message = $"User with ID {id} not found." });

                await _libraryService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while deleting the user.");
            }
        }

        private IActionResult HandleException(Exception ex, string message)
        {
            // Log the exception (log integration can be added here)
            Console.WriteLine(ex);

            return StatusCode(500, new
            {
                Message = message,
                Error = ex.Message
            });
        }
    }
}