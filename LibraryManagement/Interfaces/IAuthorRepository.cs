using LibraryManagement.Models;

namespace LibraryManagement.Interfaces
{
    public interface IAuthorRepository
    {
        public Task<IEnumerable<Author>> GetAuthorsAsync();
        public Task<Author?> GetAuthorAsync(int id);
        public Task<bool> AuthorExistsAsync(int id);
        public Task<int> SaveAuthorAsync();
        public Task<int> AddAuthorAsync(Author author);
        public Task<int> RemoveAuthorAsync(Author author);
        public void ChangeEntityState(Author author);
    }
}
