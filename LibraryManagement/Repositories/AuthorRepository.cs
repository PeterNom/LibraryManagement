using LibraryManagement.Data;
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LibraryManagement.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibManagerDbContext _libManagerDbcontext;

        public AuthorRepository(LibManagerDbContext libManagerDbcontext)
        {
            _libManagerDbcontext = libManagerDbcontext;
        }

        // Get author by id.
        public async Task<Author?> GetAuthorAsync(int id)
        {
            
            return await _libManagerDbcontext.Authors.Include(b=>b.Books).AsNoTracking().FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        // Get all authors.
        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _libManagerDbcontext.Authors.Include(b=>b.Books).OrderBy(a => a.Name).AsNoTracking().ToListAsync();
        }

        // Check if the author exists.
        public async Task<bool> AuthorExistsAsync(int id)
        {
            var result = await _libManagerDbcontext.Authors.AsNoTracking().AnyAsync(a => a.AuthorId == id);

            return result;
        }

        // Save changes to the database.
        public async Task<int> SaveAuthorAsync()
        {
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        // Add author to the database and Save changes.
        public async Task<int> AddAuthorAsync(Author author)
        {
            _libManagerDbcontext.Authors.Add(author);
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        // Remove author from the database and Save changes.
        public async Task<int> RemoveAuthorAsync(Author author)
        {
            _libManagerDbcontext.Authors.Remove(author);
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        // Update the state of the EntityState when updating the database.
        public void ChangeEntityState(Author author)
        {
            _libManagerDbcontext.Entry(author).State = EntityState.Modified;
        }
    }
}
