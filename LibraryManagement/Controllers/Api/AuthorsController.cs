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
    public class AuthorsController : ControllerBase
    {
        private readonly LibManagerDbContext _libManagerDbcontext;

        public AuthorsController(LibManagerDbContext context)
        {
            _libManagerDbcontext = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _libManagerDbcontext.Authors.OrderBy(a => a.Name).ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _libManagerDbcontext.Authors.FirstOrDefaultAsync(a =>a.AuthorId==id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest();
            }

            _libManagerDbcontext.Entry(author).State = EntityState.Modified;

            try
            {
                await _libManagerDbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var result = _libManagerDbcontext.Authors.Any(a => a.AuthorId == id);

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
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            _libManagerDbcontext.Authors.Add(author);
            await _libManagerDbcontext.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.AuthorId }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _libManagerDbcontext.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
            
            if (author == null)
            {
                return NotFound();
            }

            _libManagerDbcontext.Authors.Remove(author);
            await _libManagerDbcontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
