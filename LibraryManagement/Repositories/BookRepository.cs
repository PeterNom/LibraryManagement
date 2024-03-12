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

        // Get book by id.
        public async Task<Book?> GetBookAsync(int id)
        {
            return await _libManagerDbcontext.Books.Include(a => a.Author).FirstOrDefaultAsync(a => a.BookId == id);
        }

        // Get all books.
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _libManagerDbcontext.Books.Include(a=>a.Author).OrderBy(a => a.Title).ToListAsync();
        }

        // Get book by ISBN.
        public async Task<Book?> GetBookByISBNAsync(string isbn)
        {
            return await _libManagerDbcontext.Books.Include(a => a.Author).FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        // Get book by title.
        public async Task<Book?> GetBookByTitleAsync(string title)
        {
            return await _libManagerDbcontext.Books.Include(a => a.Author).FirstOrDefaultAsync(b => b.Title == title);
        }

        // Check if the book exists.
        public async Task<bool> BookExistsAsync(int id)
        {
            var result = await _libManagerDbcontext.Books.Include(a => a.Author).AnyAsync(a => a.BookId == id);

            return result;
        }

        // Save changes to the database.
        public async Task<int> SaveBookAsync()
        {
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        // Add book to the database and Save changes.
        public async Task<int> AddBookAsync(Book book)
        {
            _libManagerDbcontext.Books.Add(book);
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        // Remove book from the database and Save changes.
        public async Task<int> RemoveBookAsync(Book book)
        {
            _libManagerDbcontext.Books.Remove(book);
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        // Update the state of the EntityState when updating the database. 
        public void ChangeEntityState(Book book)
        {
            _libManagerDbcontext.Entry(book).State = EntityState.Modified;
        }
    }
}
