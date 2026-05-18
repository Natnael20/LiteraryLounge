using LiteraryLounge.API.Models;
using LiteraryLounge.API.Constants;
using LiteraryLounge.API.Repository;

namespace LiteraryLounge.API.Services;

/// <summary>
/// Handles business logic for quote operations including validation and duplicate checking.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class QuoteService  
{
    private readonly QuoteRepository _repository;

    /// <summary>
    /// Initializes a new instance of the QuoteService.
    /// </summary>
    /// <param name="repository">The quote repository for data access.</param>
    public QuoteService(QuoteRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets all quotes from the repository.
    /// </summary>
    /// <returns>List of all quotes.</returns>
    public List<Quote> GetAll() 
    {
        return _repository.GetAll();
    }
    
    /// <summary>
    /// Gets a specific quote by its ID.
    /// </summary>
    /// <param name="id">The quote ID.</param>
    /// <returns>The matching quote, or null if not found.</returns>
    public Quote GetById(int id) 
    {
        return _repository.GetById(id);
    }
    
    /// <summary>
    /// Creates a new quote after validation and duplicate check.
    /// </summary>
    /// <param name="quote">The quote to create.</param>
    /// <returns>Success status, message, and created quote.</returns>
    public (bool Success, string Message, Quote Quote) Create(Quote quote) 
    {
        var validation = ValidateQuoteFields(quote);
        if (!validation.IsValid)
        {
            return (false, validation.Message, null);
        }
        
        if (IsDuplicate(quote.Text, quote.Author))
        {
            return (false, string.Format(QuoteMessages.QuoteExists, quote.Text, quote.Author), null);
        }
        
        var created = _repository.Create(quote);
        return (true, QuoteMessages.QuoteCreated, created);
    }
    
    /// <summary>
    /// Updates an existing quote by ID.
    /// </summary>
    /// <param name="id">ID of the quote to update.</param>
    /// <param name="quote">The updated quote data.</param>
    /// <returns>Success status, message, and updated quote.</returns>
    public (bool Success, string Message, Quote Quote) Update(int id, Quote quote) 
    {
        var existing = _repository.GetById(id);
        if (existing == null)
        {
            return (false, QuoteMessages.QuoteNotFound, null);
        }
        
        var validation = ValidateQuoteFields(quote);
        if (!validation.IsValid)
        {
            return (false, validation.Message, null);
        }
        
        if (IsDuplicate(quote.Text, quote.Author, id))
        {
            return (false, string.Format(QuoteMessages.QuoteExists, quote.Text, quote.Author), null);
        }
        
        var updated = _repository.Update(id, quote);
        return (true, QuoteMessages.QuoteUpdated, updated);
    }
    
    /// <summary>
    /// Deletes a quote by ID.
    /// </summary>
    /// <param name="id">ID of the quote to delete.</param>
    /// <returns>Success status and message.</returns>
    public (bool Success, string Message) Delete(int id) 
    {
        var deleted = _repository.Delete(id);
        return deleted ? (true, QuoteMessages.QuoteDeleted) : (false, QuoteMessages.QuoteNotFound);
    }
    
    /// <summary>
    /// Validates that quote text and author are not empty or whitespace.
    /// </summary>
    /// <param name="quote">The quote to validate.</param>
    /// <returns>IsValid flag and error message if invalid.</returns>
    private (bool IsValid, string Message) ValidateQuoteFields(Quote quote)
    {
        if (string.IsNullOrWhiteSpace(quote.Text))
            return (false, QuoteMessages.TextRequired);
        
        if (string.IsNullOrWhiteSpace(quote.Author))
            return (false, QuoteMessages.AuthorRequired);
        
        return (true, string.Empty);
    }
    
    /// <summary>
    /// Checks if a quote with the same text and author already exists.
    /// </summary>
    /// <param name="text">The quote text.</param>
    /// <param name="author">The quote author.</param>
    /// <param name="excludeId">Optional ID to exclude (used during update).</param>
    /// <returns>True if duplicate exists, otherwise false.</returns>
    private bool IsDuplicate(string text, string author, int? excludeId = null)
    {
        var allQuotes = _repository.GetAll();
        foreach (var quote in allQuotes)
        {
            bool isTextMatch = quote.Text.ToLower() == text.ToLower();
            bool isAuthorMatch = quote.Author.ToLower() == author.ToLower();
            
            if (excludeId.HasValue && quote.Id == excludeId.Value)
                continue;
            
            if (isTextMatch && isAuthorMatch)
                return true;
        }
        return false;
    }
}