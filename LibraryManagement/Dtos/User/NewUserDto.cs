using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Dtos.User
{
    public class NewUserDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
