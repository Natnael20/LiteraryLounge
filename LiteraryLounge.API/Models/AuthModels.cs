
namespace LiteraryLounge.API.Models;

/// <summary>
/// Models for authentication requests and responses, including registration,
///  login, and JWT-based authentication response.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
}