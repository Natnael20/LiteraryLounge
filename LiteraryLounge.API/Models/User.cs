namespace LiteraryLounge.API.Models;

/// <summary>
/// Represents a user in the Literary Lounge application.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class User
{

    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}