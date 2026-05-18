namespace LiteraryLounge.API.Models;

/// <summary>
/// Represents a literary quote in the application.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class Quote
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty; // Optional
}