using Asp.Versioning;
using BookLibraryAPI.Data;
using BookLibraryAPI.Models;
using BookLibraryAPI.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BookLibraryAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BooksController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet("protected")]
        [Authorize]
        public IActionResult GetProtected()
        {
            return Ok("This is a protected endpoint!");
        }

        /// <summary>
        /// Retrieves a list of all books.
        /// </summary>
        /// <returns>A list of books.</returns>
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await context.Books.ToListAsync();
            return Ok(books);
        }

        /// <summary>
        /// Retrieves a specific book by unique id.
        /// </summary>
        /// <param name="id">The id of the book.</param>
        /// <returns>A book.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        /// <summary>
        /// Searches for books based on given parameters.
        /// </summary>
        /// <param name="parameters">The search parameters.</param>
        /// <returns>A list of books that match the search criteria.</returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchBooks([FromQuery] SearchBooksParameters parameters)
        {
            var query = context.Books.AsQueryable();

            query = ApplyFilters(query, parameters);

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
                parameters.PageNumber,
                parameters.PageSize,
                TotalPages = totalPages,
                Items = books
            };

            return Ok(response);
        }

        /// <summary>
        /// Applies filters to the books query.
        /// </summary>
        /// <param name="query">The books query.</param>
        /// <param name="parameters">The search parameters.</param>
        /// <returns>The filtered query.</returns>
        private IQueryable<Book> ApplyFilters(IQueryable<Book> query, SearchBooksParameters parameters)
        {
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

            return query;
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="book">The book to create.</param>
        /// <returns>The created book.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBook(Book book)
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The id of the book to update.</param>
        /// <param name="updatedBook">The updated book.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.Author = updatedBook.Author;
            book.Genre = updatedBook.Genre;
            book.IsBorrowed = updatedBook.IsBorrowed;
            await context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a book.
        /// </summary>
        /// <param name="id">The id of the book to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            context.Books.Remove(book);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
