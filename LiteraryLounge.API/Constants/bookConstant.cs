namespace LiteraryLounge.API.Constants;

/// <author>Natnael Yonas Weldetensae</author>
public static class BookMessages
{
    // Success messages
    public const string BookCreated = "Book created successfully";
    public const string BookUpdated = "Book updated successfully";
    public const string BookDeleted = "Book deleted successfully";
    
    // Error messages
    public const string BookNotFound = "Book not found";
    public const string TitleRequired = "Title is required";
    public const string AuthorRequired = "Author is required";
    public const string ISBNRequired = "ISBN is required";
    public const string ISBNInvalid = "Invalid ISBN format. ISBN must be 10 or 13 digits";
    public const string InvalidPublishYear = "Valid publish year is required (between 1000 and current year)";
    
    // Duplicate message template
    public const string BookExists = "Book '{0}' by '{1}' already exists";
    public const string ISBNExists = "Book with ISBN '{0}' already exists";
}