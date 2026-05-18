namespace LiteraryLounge.API.Models;

/// <summary>
/// Contains models for the Literary Lounge application.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty; 
    public int PublishYear { get; set; }
}