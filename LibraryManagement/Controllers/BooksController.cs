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

namespace LibraryManagement.Controllers
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
            // Return all books.
            var books = await _bookRepository.GetBooksAsync();

            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            // Find the book to return.
            var book = await _bookRepository.GetBookAsync(id);

            // Check if book found.
            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            // Check if we update the correct object.
            if (id != book.BookId)
            {
                return BadRequest();
            }

            // Notify the entity state that the object state had changed.
            _bookRepository.ChangeEntityState(book);

            try
            {
                // Update book to the database.
                await _bookRepository.SaveBookAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the book exists.
                var result = await _bookRepository.BookExistsAsync(id);

                // We tried to update book that doesn't exists.
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
            // Check if book to add is not null.
            if (book == null)
                return BadRequest();

            // Retrieve the author of the book.
            Author? author = await _authorRepository.GetAuthorAsync(book.AuthorId);

            // Check that the author is not null.
            if (author == null)
                return BadRequest();

            // Update book author.
            book.Author = author;

            // Add book to the database.
            await _bookRepository.AddBookAsync(book);

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            // Find the book to delete.
            var book = await _bookRepository.GetBookAsync(id);

            // Check if book exists.
            if (book == null)
            {
                return NotFound();
            }

            // Delete book from the database.
            await _bookRepository.RemoveBookAsync(book);

            return NoContent();
        }
    }
}
