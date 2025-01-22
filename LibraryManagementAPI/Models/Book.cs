using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        //[Range(0, int.MaxValue)]
        [Required]
        public int CopiesAvailable { get; set; }

    }
}
