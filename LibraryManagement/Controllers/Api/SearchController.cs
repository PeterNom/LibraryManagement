using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly BookRepository _bookRepository;
        public SearchController(BookRepository bookRepository) 
        {
            _bookRepository = bookRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SearchBook([FromBody] string ISBN, string? Title)
        {
            Book? book = new Book();

            if (!string.IsNullOrEmpty(ISBN) && string.IsNullOrEmpty(Title))
            {
                book = await _bookRepository.GetBookByISBNAsync(ISBN);
            }
            if(string.IsNullOrEmpty(ISBN) && !string.IsNullOrEmpty(Title))
            {
                book = await _bookRepository.GetBookByTitleAsync(Title);
            }

            if (!string.IsNullOrEmpty(ISBN) && !string.IsNullOrEmpty(Title))
            {
                var both = await _bookRepository.GetBookByISBNAsync(ISBN) == await _bookRepository.GetBookByTitleAsync(Title);
                book = await _bookRepository.GetBookByISBNAsync(ISBN);
            }

            return new JsonResult(book);
        }
    }
}
