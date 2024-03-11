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

        public async Task<Author?> GetAuthorAsync(int id)
        {
            
            return await _libManagerDbcontext.Authors.Include(b=>b.Books).AsNoTracking().FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _libManagerDbcontext.Authors.Include(b=>b.Books).OrderBy(a => a.Name).AsNoTracking().ToListAsync();
        }

        public async Task<bool> AuthorExistsAsync(int id)
        {
            var result = await _libManagerDbcontext.Authors.AsNoTracking().AnyAsync(a => a.AuthorId == id);

            return result;
        }

        public async Task<int> SaveAuthorAsync()
        {
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        public async Task<int> AddAuthorAsync(Author author)
        {
            _libManagerDbcontext.Authors.Add(author);
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        public async Task<int> RemoveAuthorAsync(Author author)
        {
            _libManagerDbcontext.Authors.Remove(author);
            return await _libManagerDbcontext.SaveChangesAsync();
        }

        public void ChangeEntityState(Author author)
        {
            _libManagerDbcontext.Entry(author).State = EntityState.Modified;
        }
    }
}
