using Microsoft.AspNetCore.Mvc;
using LiteraryLounge.API.Models;
using LiteraryLounge.API.Services;

namespace LiteraryLounge.API.Controllers;

/// <summary>
/// Handles authentication requests including user registration and login.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    /// <summary>
    /// Initializes a new instance of the AuthController.
    /// </summary>
    /// <param name="authService">The authentication service.</param>
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user with username and password.
    /// </summary>
    /// <param name="request">The registration request containing username and password.</param>
    /// <returns>Success message or validation errors.</returns>
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var result = _authService.Register(request.Username, request.Password);
        
        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(new { message = result.Message });
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="request">The login request containing username and password.</param>
    /// <returns>JWT token and user information if successful, otherwise unauthorized.</returns>
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var result = _authService.Login(request.Username, request.Password);
        
        if (!result.Success)
        {
            return Unauthorized(new { message = result.Message });
        }

        return Ok(new AuthResponse
        {
            Token = result.Token,
            Username = result.User!.Username,
            UserId = result.User.Id,
            Message = result.Message
        });
    }
}