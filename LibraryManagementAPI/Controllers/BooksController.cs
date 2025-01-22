using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dto;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryService _libraryService;

        public BooksController(LibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _libraryService.GetBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging library)
                // _logger.LogError(ex, "An error occurred while fetching books");
                return StatusCode(500, "An error occurred while retrieving books. Please try again later.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = await _libraryService.GetBookByIdAsync(id);
                if (book == null) return NotFound($"Book with ID {id} not found.");
                return Ok(book);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while retrieving the book. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _libraryService.AddBookAsync(book);
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (DbUpdateException dbEx)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while adding the book to the database.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookDto updatedBook)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid book ID.");
            }

            try
            {
                var existingBook = await _libraryService.GetBookByIdAsync(id);
                if (existingBook == null) return NotFound($"Book with ID {id} not found.");

                var updatedEntity = new Book
                {
                    Id = id,
                    Title = updatedBook.Title,
                    Author = updatedBook.Author,
                    CopiesAvailable = updatedBook.CopiesAvailable
                };

                await _libraryService.UpdateBookAsync(updatedEntity);

                return Ok(updatedBook);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("The book was updated or deleted by another user.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An unexpected error occurred while updating the book. Please try again.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _libraryService.GetBookByIdAsync(id);
                if (book == null) return NotFound($"Book with ID {id} not found.");

                await _libraryService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while deleting the book. It may have related records.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An unexpected error occurred while deleting the book. Please try again.");
            }
        }
    }
}