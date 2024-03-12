using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace LibraryManagement.Dtos.User
{
    public class LoginDto
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
