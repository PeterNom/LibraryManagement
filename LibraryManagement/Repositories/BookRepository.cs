using LibraryManagement.Data;
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories
{
    public class BookRepository: IBookRepository
    {
        private readonly LibManagerDbContext _libManagerDbcontext;

        public BookRepository(LibManagerDbContext libManagerDbcontext)
        {
            _libManagerDbcontext = libManagerDbcontext;
        }

        public async Task<Book?> GetBookAsync(int id)
        {

            return await _libManagerDbcontext.Books.Include(a => a.Author).FirstOrDefaultAsync(a => a.BookId == id);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _libManagerDbcontext.Books.Include(a=>a.Author).OrderBy(a => a.Title).ToListAsync();
        }
        public async Task<Book?> GetBookByISBNAsync(string isbn)
        {
            return await _libManagerDbcontext.Books.Include(a => a.Author).FirstOrDefaultAsync(b => b.ISBN == isbn);
        }
        public async Task<Book?> GetBookByTitleAsync(string title)
        {
            return await _libManagerDbcontext.Books.Include(a => a.Author).FirstOrDefaultAsync(b => b.Title == title);
        }
        public async Task<bool> BookExistsAsync(int id)
        {
            var result = await _libManagerDbcontext.Books.Include(a => a.Author).AnyAsync(a => a.BookId == id);

            return result;
        }

        public async Task<int> SaveBookAsync()
        {
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        public async Task<int> AddBookAsync(Book book)
        {
            _libManagerDbcontext.Books.Add(book);
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        public async Task<int> RemoveBookAsync(Book book)
        {
            _libManagerDbcontext.Books.Remove(book);
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        public void ChangeEntityState(Book book)
        {
            _libManagerDbcontext.Entry(book).State = EntityState.Modified;
        }
    }
}
