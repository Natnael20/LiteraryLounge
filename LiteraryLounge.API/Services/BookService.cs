using LiteraryLounge.API.Models;
using LiteraryLounge.API.Constants;
using LiteraryLounge.API.Repository;
using System.Text.RegularExpressions;

namespace LiteraryLounge.API.Services;

/// <summary>
/// Handles business logic for book operations including validation and duplicate checking.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class BookService  
{
    private readonly BookRepository _repository;

    public BookService(BookRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets all books from the repository.
    /// </summary>
    /// <returns>List of all books.</returns>
    public List<Book> GetAll() 
    {
        return _repository.GetAll();
    }
    
    /// <summary>
    /// Gets a specific book by its ID.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>The book if found, otherwise null.</returns>
    public Book? GetById(int id) 
    {
        return _repository.GetById(id);
    }
    
    /// <summary>
    /// Creates a new book after validation and duplicate checks.
    /// </summary>
    /// <param name="book">The book to create.</param>
    /// <returns>Success status, message, and created book.</returns>
    public (bool Success, string Message, Book? Book) Create(Book book) 
    {
        // Trim and prepare the book data
        book.ISBN = book.ISBN?.Trim() ?? "";
        
        var validation = ValidateBookFields(book);
        if (!validation.IsValid)
        {
            return (false, validation.Message, null);
        }
        
        var duplicateCheck = IsDuplicate(book.Title, book.Author, book.ISBN);
        if (duplicateCheck.IsDuplicate)
        {
            return (false, duplicateCheck.Message, null);
        }
        
        var created = _repository.Create(book);
        return (true, BookMessages.BookCreated, created);
    }
    
    /// <summary>
    /// Updates an existing book. ISBN cannot be changed.
    /// </summary>
    /// <param name="id">ID of the book to update.</param>
    /// <param name="book">The updated book data.</param>
    /// <returns>Success status, message, and updated book.</returns>
    public (bool Success, string Message, Book? Book) Update(int id, Book book) 
    {
        var existing = _repository.GetById(id);
        if (existing == null)
        {
            return (false, BookMessages.BookNotFound, null);
        }
        
        // Trim and prepare the book data for validation
        book.ISBN = book.ISBN?.Trim() ?? "";
        
        var validation = ValidateBookFields(book);
        if (!validation.IsValid)
        {
            return (false, validation.Message, null);
        }
        
        var duplicateCheck = IsDuplicate(book.Title, book.Author, book.ISBN, id);
        if (duplicateCheck.IsDuplicate)
        {
            return (false, duplicateCheck.Message, null);
        }
        
        // Preserve original ISBN - cannot be updated
        book.ISBN = existing.ISBN;
        
        var updated = _repository.Update(id, book);
        return (true, BookMessages.BookUpdated, updated);
    }
    
    /// <summary>
    /// Deletes a book by ID.
    /// </summary>
    /// <param name="id">ID of the book to delete.</param>
    /// <returns>Success status and message.</returns>
    public (bool Success, string Message) Delete(int id) 
    {
        var deleted = _repository.Delete(id);
        if (deleted)
        {
            return (true, BookMessages.BookDeleted);
        }
        return (false, BookMessages.BookNotFound);
    }
    
    /// <summary>
    /// Validates all book fields (title, author, ISBN format, publish year).
    /// </summary>
    /// <param name="book">The book to validate.</param>
    /// <returns>IsValid flag and error message if invalid.</returns>
    private (bool IsValid, string Message) ValidateBookFields(Book book) 
    {
        if (string.IsNullOrWhiteSpace(book.Title))
        {
            return (false, BookMessages.TitleRequired);
        }
        
        if (string.IsNullOrWhiteSpace(book.Author))
        {
            return (false, BookMessages.AuthorRequired);
        }
        
        if (string.IsNullOrWhiteSpace(book.ISBN))
        {
            return (false, BookMessages.ISBNRequired);
        }
        
        // Validate ISBN format (supports 10-digit and 13-digit with or without hyphens/spaces)
        string cleanIsbn = Regex.Replace(book.ISBN.Trim(), @"[-\s]", "");
        bool isValidISBN = Regex.IsMatch(cleanIsbn, @"^\d{13}$") || 
                          Regex.IsMatch(cleanIsbn, @"^\d{9}[\dXx]$");
        
        if (!isValidISBN)
        {
            return (false, BookMessages.ISBNInvalid);
        }
        
        // Validate publish year
        int currentYear = DateTime.Now.Year;
        if (book.PublishYear < 1000 || book.PublishYear > currentYear)
        {
            return (false, BookMessages.InvalidPublishYear);
        }
        
        return (true, string.Empty);
    }
    
    /// <summary>
    /// Checks for duplicate books by ISBN or Title+Author combination.
    /// </summary>
    /// <param name="title">Book title.</param>
    /// <param name="author">Book author.</param>
    /// <param name="isbn">Book ISBN.</param>
    /// <param name="excludeId">Optional ID to exclude (used during update).</param>
    /// <returns>IsDuplicate flag and message if duplicate found.</returns>
    private (bool IsDuplicate, string Message) IsDuplicate(string title, string author, string isbn, int? excludeId = null)
    {
        foreach (var existing in _repository.GetAll())
        {
            // Skip the current book when updating
            if (excludeId.HasValue && existing.Id == excludeId.Value)
            {
                continue;
            }
            
            // Check ISBN duplicate
            if (existing.ISBN == isbn)
            {
                return (true, string.Format(BookMessages.ISBNExists, isbn));
            }
            
            // Check Title and Author duplicate (case-insensitive)
            if (existing.Title.ToLower() == title.ToLower() && 
                existing.Author.ToLower() == author.ToLower())
            {
                return (true, string.Format(BookMessages.BookExists, title, author));
            }
        }
        
        return (false, string.Empty);
    }
}