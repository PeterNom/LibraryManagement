using LibraryManagement.Models;

namespace LibraryManagement.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
