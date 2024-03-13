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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            // Return all authors.
            var authors = await _authorRepository.GetAuthorsAsync();

            return Ok(authors);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            // Find the author to return.
            var author = await _authorRepository.GetAuthorAsync(id);

            // Check if author found.
            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            // Check if we update the correct object.
            if (id != author.AuthorId)
            {
                return BadRequest();
            }

            // Notify the entity state that the object state had changed.
            _authorRepository.ChangeEntityState(author);

            try
            {
                // Update author to the database.
                await _authorRepository.SaveAuthorAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the author exists.
                var result = await _authorRepository.AuthorExistsAsync(id);

                // We tried to update author that doesn't exists.
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

        // POST: api/Authors
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            // Check if author to add is not null.
            if (author == null)
                return BadRequest();

            // Add author to the database.
            await _authorRepository.AddAuthorAsync(author);

            return CreatedAtAction("GetAuthor", new { id = author.AuthorId }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            // Find the author to delete.
            var author = await _authorRepository.GetAuthorAsync(id);

            // Check if author exists.
            if (author == null)
            {
                return NotFound();
            }

            // Delete author from the database.
            await _authorRepository.RemoveAuthorAsync(author);

            return NoContent();
        }
    }
}
