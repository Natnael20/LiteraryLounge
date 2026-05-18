using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LiteraryLounge.API.Models;
using LiteraryLounge.API.Services;
using LiteraryLounge.API.Constants;

namespace LiteraryLounge.API.Controllers;

/// <summary>
/// Handles HTTP requests for quote operations. All endpoints require authentication.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
[ApiController]
[Route("api/[controller]")]
[Authorize]  // Add this
public class QuotesController : ControllerBase
{
    private readonly QuoteService _quoteService;
    
    /// <summary>
    /// Initializes a new instance of the QuotesController.
    /// </summary>
    /// <param name="quoteService">The quote service for business logic.</param>
    public QuotesController(QuoteService quoteService)
    {
        _quoteService = quoteService;
    }
    
    /// <summary>
    /// Retrieves all quotes.
    /// </summary>
    /// <returns>List of all quotes.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_quoteService.GetAll());
    }
    
    /// <summary>
    /// Retrieves a specific quote by ID.
    /// </summary>
    /// <param name="id">The quote ID.</param>
    /// <returns>The matching quote or not found.</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var quote = _quoteService.GetById(id);
        if (quote == null)
        {
            return NotFound(new { message = QuoteMessages.QuoteNotFound });
        }
        return Ok(quote);
    }
    
    /// <summary>
    /// Creates a new quote.
    /// </summary>
    /// <param name="quote">The quote to create.</param>
    /// <returns>The created quote with its location.</returns>
    [HttpPost]
    public IActionResult Create(Quote quote)
    {
        var result = _quoteService.Create(quote);
        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }
        return CreatedAtAction(nameof(GetById), new { id = result.Quote.Id }, result.Quote);
    }
    
    /// <summary>
    /// Updates an existing quote by ID.
    /// </summary>
    /// <param name="id">The ID of the quote to update.</param>
    /// <param name="quote">The updated quote data.</param>
    /// <returns>The updated quote or bad request.</returns>
    [HttpPut("{id}")]
    public IActionResult Update(int id, Quote quote)
    {
        var result = _quoteService.Update(id, quote);
        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }
        return Ok(result.Quote);
    }
    
    /// <summary>
    /// Deletes a quote by ID.
    /// </summary>
    /// <param name="id">The ID of the quote to delete.</param>
    /// <returns>No content if successful, otherwise not found.</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _quoteService.Delete(id);
        if (!result.Success)
        {
            return NotFound(new { message = result.Message });
        }
        return NoContent();
    }
}