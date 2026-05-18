using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LiteraryLounge.API.Models;
using LiteraryLounge.API.Services;
using LiteraryLounge.API.Constants;

namespace LiteraryLounge.API.Controllers;

/// <summary>
/// Handles HTTP requests for book operations.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
[ApiController]
[Route("api/[controller]")]
[Authorize]  // Add this to protect all endpoints
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;
    
    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }
    
    /// <summary>
    /// Retrieves all books.
    /// </summary>
    /// <returns>List of all books.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var books = _bookService.GetAll();
        return Ok(books);
    }
    
    /// <summary>
    /// Retrieves a specific book by ID.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>The matching book or not found.</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var book = _bookService.GetById(id);
        if (book == null)
        {
            return NotFound(new { message = BookMessages.BookNotFound });
        }
        return Ok(book);
    }
    
    /// <summary>
    /// Creates a new book.
    /// </summary>
    /// <param name="book">The book to create.</param>
    /// <returns>The created book with its location.</returns>
    [HttpPost]
    public IActionResult Create(Book book)
    {
        var result = _bookService.Create(book);
        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }
        return CreatedAtAction(nameof(GetById), new { id = result.Book.Id }, result.Book);
    }
    
    /// <summary>
    /// Updates an existing book by ID.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="book">The updated book data.</param>
    /// <returns>The updated book or bad request.</returns>
    [HttpPut("{id}")]
    public IActionResult Update(int id, Book book)
    {
        var result = _bookService.Update(id, book);
        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }
        return Ok(result.Book);
    }
    
    /// <summary>
    /// Deletes a book by ID.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    /// <returns>No content if successful, otherwise not found.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _bookService.Delete(id);
        if (!result.Success)
        {
            return NotFound(new { message = result.Message });
        }
        return NoContent();
    }
}