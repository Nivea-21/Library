using System.ComponentModel.DataAnnotations;
using LibraryManagementAPI.Models;

public class Loan
{
    public int Id { get; set; }
    
    // Foreign key properties
    [Required]
    public int BookId { get; set; }

    [Required]
    public int UserId { get; set; }

    // Date properties
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
