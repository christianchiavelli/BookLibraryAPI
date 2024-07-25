using BookLibraryAPI.Data;
using BookLibraryAPI.Models;
using BookLibraryAPI.Parameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] SearchBooksParameters parameters)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Title))
            {
                query = query.Where(b => b.Title.Contains(parameters.Title));
            }
            if (!string.IsNullOrEmpty(parameters.Description))
            {
                query = query.Where(b => b.Description.Contains(parameters.Description));
            }
            if (!string.IsNullOrEmpty(parameters.Author))
            {
                query = query.Where(b => b.Author.Contains(parameters.Author));
            }
            if (!string.IsNullOrEmpty(parameters.Genre))
            {
                query = query.Where(b => b.Genre.Contains(parameters.Genre));
            }
            if (parameters.StartDate.HasValue && parameters.EndDate.HasValue)
            {
                query = query.Where(b => b.PublicationDate >= parameters.StartDate && b.PublicationDate <= parameters.EndDate);
            }
            if (parameters.IsBorrowed.HasValue)
            {
                query = query.Where(b => b.IsBorrowed == parameters.IsBorrowed.Value);
            }
            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(b => b.Price >= parameters.MinPrice.Value);
            }
            if (parameters.MaxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= parameters.MaxPrice.Value);
            }
            if (parameters.MinRating.HasValue)
            {
                query = query.Where(b => b.Rating >= parameters.MinRating.Value);
            }
            if (parameters.MaxRating.HasValue)
            {
                query = query.Where(b => b.Rating <= parameters.MaxRating.Value);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize);

            var books = await query
                .OrderBy(parameters.SortBy)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var response = new
            {
                TotalItems = totalItems,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = totalPages,
                Items = books
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.Author = updatedBook.Author;
            book.Genre = updatedBook.Genre;
            book.IsBorrowed = updatedBook.IsBorrowed;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
