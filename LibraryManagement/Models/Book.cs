using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; } = string.Empty ;

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
    }
}
