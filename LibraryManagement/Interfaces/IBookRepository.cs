using LibraryManagement.Models;

namespace LibraryManagement.Interfaces
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetBooksAsync();
        public Task<Book?> GetBookAsync(int id);
        public Task<bool> BookExistsAsync(int id);
        public Task<int> SaveBookAsync();
        public Task<int> AddBookAsync(Book book);
        public Task<int> RemoveBookAsync(Book book);
        public void ChangeEntityState(Book book);
        public Task<Book?> GetBookByISBNAsync(string isbn);
        public Task<Book?> GetBookByTitleAsync(string title);
    }
}
