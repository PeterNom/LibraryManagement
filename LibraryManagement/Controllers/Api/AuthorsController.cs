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
            var authors = await _authorRepository.GetAuthorsAsync();
            return Ok(authors);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _authorRepository.GetAuthorAsync(id);

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
            if (id != author.AuthorId)
            {
                return BadRequest();
            }
            _authorRepository.ChangeEntityState(author);

            try
            {
                await _authorRepository.SaveAuthorAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = await _authorRepository.AuthorExistsAsync(id);

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
        public async Task<ActionResult<Author>> PostAuthor([FromBody] Author author)
        {
            await _authorRepository.AddAuthorAsync(author);

            return CreatedAtAction("GetAuthor", new { id = author.AuthorId }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorRepository.GetAuthorAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            await _authorRepository.RemoveAuthorAsync(author);

            return NoContent();
        }
    }
}
