using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LoansController(LibraryContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetLoans()
        {
            try
            {
                var loans = await _context.Loans
                    .Select(l => new
                    {
                        l.Id,
                        l.BookId,
                        l.UserId,
                        l.LoanDate,
                        l.ReturnDate,
                        Book = new
                        {
                            l.BookId,
                            // Include additional Book details if necessary
                        },
                        User = new
                        {
                            l.UserId,
                            // Include additional User details if necessary
                        }
                    })
                    .ToListAsync();

                return Ok(loans);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while retrieving loans.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var loan = await _context.Loans
                    .Where(l => l.Id == id)
                    .Select(l => new
                    {
                        l.Id,
                        l.BookId,
                        l.UserId,
                        l.LoanDate,
                        l.ReturnDate,
                        Book = new
                        {
                            l.BookId,
                            // Include additional Book details if necessary
                        },
                        User = new
                        {
                            l.UserId,
                            // Include additional User details if necessary
                        }
                    })
                    .FirstOrDefaultAsync();

                if (loan == null)
                    return NotFound(new { Message = $"Loan with ID {id} not found." });

                return Ok(loan);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while retrieving the loan.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddLoan(Loan loan)
        {
            try
            {
                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLoans), new { id = loan.Id }, loan);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while adding the loan.");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, Loan updatedLoan)
        {
            try
            {
                if (id != updatedLoan.Id)
                    return BadRequest(new { Message = "Loan ID mismatch." });

                _context.Update(updatedLoan);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Loans.Any(l => l.Id == id))
                        return NotFound(new { Message = $"Loan with ID {id} not found." });

                    throw; // Rethrow if it is a concurrency exception
                }

                return Ok(updatedLoan);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while updating the loan.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                var loan = await _context.Loans.FindAsync(id);
                if (loan == null)
                    return NotFound(new { Message = $"Loan with ID {id} not found." });

                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while deleting the loan.");
            }
        }
        private IActionResult HandleException(Exception ex, string message)
        {
            // Log the exception (replace with a proper logging library in production)
            Console.WriteLine(ex);

            // Return HTTP 500 with a custom error message and exception details
            return StatusCode(500, new
            {
                Message = message,
                Error = ex.Message
            });
        }
    }
}