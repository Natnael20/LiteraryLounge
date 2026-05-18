using LiteraryLounge.API.Models;
using LiteraryLounge.API.Repository;
using LiteraryLounge.API.Constants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace LiteraryLounge.API.Services;

/// <summary>
/// Handles user authentication including registration, login, and JWT token generation.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class AuthService
{
    private readonly UserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(UserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// Registers a new user with username and password.
    /// </summary>
    /// <param name="username">The username to register.</param>
    /// <param name="password">The password to register.</param>
    /// <returns>Success status, message, and created user.</returns>
    public (bool Success, string Message, User? User) Register(string username, string password)
    {
        // Check if username exists
        if (_userRepository.GetByUsername(username) != null)
        {
            return (false, authConstant.USERNAME_EXISTS, null);
        }

        // Validate password strength
        var passwordValidation = ValidatePassword(password);
        if (!passwordValidation.IsValid)
        {
            return (false, passwordValidation.Message, null);
        }

        // Hash password
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        // Create user
        var user = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.Now
        };

        var created = _userRepository.Create(user);
        return (true, authConstant.REGISTER_SUCCESS, created);
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="username">The username to log in.</param>
    /// <param name="password">The password to log in.</param>
    /// <returns>Success status, message, token, and authenticated user.</returns>
    public (bool Success, string Message, string Token, User? User) Login(string username, string password)
    {
        // Find user
        var user = _userRepository.GetByUsername(username);
        if (user == null)
        {
            return (false, authConstant.INVALID_CREDENTIALS, null, null);
        }
        
        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return (false, authConstant.INVALID_CREDENTIALS, null, null);
        }

        // Generate token
        var token = GenerateJwtToken(user);
        return (true, authConstant.LOGIN_SUCCESS, token, user);
    }

    /// <summary>
    /// Validates password strength against security rules.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>IsValid flag and error message if invalid.</returns>
    private (bool IsValid, string Message) ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return (false, authConstant.PASSWORD_REQUIRED);
        }
        
        var validationRules = new (Func<string, bool> Rule, string ErrorMessage)[]
        {
            (p => p.Length >= 8, authConstant.PASSWORD_MIN_LENGTH),
            (p => p.Length <= 50, authConstant.PASSWORD_MAX_LENGTH),
            (p => Regex.IsMatch(p, @"[A-Z]"), authConstant.PASSWORD_UPPERCASE),
            (p => Regex.IsMatch(p, @"[a-z]"), authConstant.PASSWORD_LOWERCASE),
            (p => Regex.IsMatch(p, @"[0-9]"), authConstant.PASSWORD_NUMBER),
            (p => Regex.IsMatch(p, @"[!@#$%^&*(),.?:{}|<>]"), authConstant.PASSWORD_SPECIAL)
        };

        foreach (var (rule, errorMessage) in validationRules)
        {
            if (!rule(password))
            {
                return (false, errorMessage);
            }
        }
        
        // Check for common patterns
        string[] commonPatterns = { "123456", "password", "qwerty", "abc123", "admin", "letmein" };
        foreach (var pattern in commonPatterns)
        {
            if (password.ToLower().Contains(pattern))
            {
                return (false, authConstant.PASSWORD_WEAK_PATTERN);
            }
        }
        
        return (true, string.Empty);
    }

    /// <summary>
    /// Generates a JWT token for an authenticated user.
    /// </summary>
    /// <param name="user">The authenticated user.</param>
    /// <returns>JWT token string.</returns>
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "SuperSecretKeyThatIsAtLeast32CharactersLong!"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "LiteraryLounge",
            audience: _configuration["Jwt:Audience"] ?? "LiteraryLoungeUsers",
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}