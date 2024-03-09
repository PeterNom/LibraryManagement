using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public class LibManagerDbContext: DbContext
    {
        public LibManagerDbContext(DbContextOptions<LibManagerDbContext> options) : base(options) 
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
