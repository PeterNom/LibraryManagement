using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace LibraryManagement.Models
{
    // Making the name of the Author unique
    [Index(nameof(Name), IsUnique = true)]
    public class Author
    {
        public int AuthorId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
