using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace LibraryManagement.Dtos.User
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
