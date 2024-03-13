using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
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
            // Book to return from the search.
            Book? book = new Book();

            // If only ISBN given 
            if (!string.IsNullOrEmpty(ISBN) && string.IsNullOrEmpty(Title))
            {
                // Find the book based on its ISBN
                book = await _bookRepository.GetBookByISBNAsync(ISBN);
            }
            // If only Title given 
            else if (string.IsNullOrEmpty(ISBN) && !string.IsNullOrEmpty(Title))
            {
                // Find the book based on its Title
                book = await _bookRepository.GetBookByTitleAsync(Title);
            }
            // If ISBN and Title given
            else if (!string.IsNullOrEmpty(ISBN) && !string.IsNullOrEmpty(Title))
            {
                // Find both books and check if they match 
                var both = await _bookRepository.GetBookByISBNAsync(ISBN) == await _bookRepository.GetBookByTitleAsync(Title);
                book = await _bookRepository.GetBookByISBNAsync(ISBN);
            }

            return new JsonResult(book);
        }
    }
}
