using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibManagerDbContext _libManagerDbcontext;

        public BooksController(LibManagerDbContext context)
        {
            _libManagerDbcontext = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _libManagerDbcontext.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _libManagerDbcontext.Books.FirstOrDefaultAsync(bk => bk.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            _libManagerDbcontext.Entry(book).State = EntityState.Modified;

            try
            {
                await _libManagerDbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = _libManagerDbcontext.Books.Any(bk => bk.BookId == id);

                if (!result)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _libManagerDbcontext.Books.Add(book);
            await _libManagerDbcontext.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _libManagerDbcontext.Books.FirstOrDefaultAsync(bk => bk.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            _libManagerDbcontext.Books.Remove(book);
            await _libManagerDbcontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
