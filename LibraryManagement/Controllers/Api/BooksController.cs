using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        public BooksController(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return Ok( await _bookRepository.GetBooksAsync() );
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookRepository.GetBookAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }
            _bookRepository.ChangeEntityState(book);

            try
            {

                await _bookRepository.SaveBookAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = await _bookRepository.BookExistsAsync(id);

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
        [Authorize]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (book == null)
                return BadRequest();

            book.Author = await _authorRepository.GetAuthorAsync(book.AuthorId);

            if (book.Author == null)
                return BadRequest();

            await _bookRepository.AddBookAsync(book);

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetBookAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            await _bookRepository.RemoveBookAsync(book);

            return NoContent();
        }
    }
}
