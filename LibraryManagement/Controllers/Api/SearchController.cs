using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly LibManagerDbContext _libManagerDbcontext;
        public SearchController(LibManagerDbContext context) 
        {
            _libManagerDbcontext = context;
        }

        [HttpPost]
        public async Task<IActionResult> SearchBook([FromBody] string ISBN, string? Title)
        {
            Book book = new Book();

            if (!string.IsNullOrEmpty(ISBN) && string.IsNullOrEmpty(Title))
            {
                book = await _libManagerDbcontext.Books.FirstOrDefaultAsync(bk => bk.ISBN == ISBN);
            }
            if(string.IsNullOrEmpty(ISBN) && !string.IsNullOrEmpty(Title))
            {
                book = await _libManagerDbcontext.Books.FirstOrDefaultAsync(bk => bk.Title == Title);
            }

            if (!string.IsNullOrEmpty(ISBN) && !string.IsNullOrEmpty(Title))
            {
                book = await _libManagerDbcontext.Books.FirstOrDefaultAsync(bk => bk.Title == Title && bk.ISBN == ISBN);
            }

            return new JsonResult(book);
        }
    }
}
