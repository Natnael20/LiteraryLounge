namespace LiteraryLounge.API.Constants;

/// <author>Natnael Yonas Weldetensae</author>
public static class QuoteMessages
{
    // Success messages
    public const string QuoteCreated = "Quote created successfully";
    public const string QuoteUpdated = "Quote updated successfully";
    public const string QuoteDeleted = "Quote deleted successfully";
    
    // Error messages
    public const string QuoteNotFound = "Quote not found";
    public const string TextRequired = "Quote text is required";
    public const string AuthorRequired = "Author is required";
    
    // Duplicate message template
    public const string QuoteExists = "Quote '{0}' by '{1}' already exists";
}