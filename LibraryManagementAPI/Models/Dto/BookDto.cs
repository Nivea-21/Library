using System.ComponentModel.DataAnnotations;

namespace Dto
{
    
public class BookDto
{
[Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        //[Range(0, int.MaxValue)]
        [Required]
        public int CopiesAvailable { get; set; }
}
}