namespace LiteraryLounge.API.Constants;

/// <author>Natnael Yonas Weldetensae</author>
public static class authConstant 
{
    // Username constants
    public const string USERNAME_EXISTS = "Username already exists";
    
    // Registration constants
    public const string REGISTER_SUCCESS = "User registered successfully";
    
    // Login constants
    public const string LOGIN_SUCCESS = "Login successful";
    public const string INVALID_CREDENTIALS = "Invalid username or password";
    
    // Password validation constants
    public const string PASSWORD_REQUIRED = "Password is required";
    public const string PASSWORD_MIN_LENGTH = "Password must be at least 8 characters long";
    public const string PASSWORD_MAX_LENGTH = "Password must not exceed 50 characters";
    public const string PASSWORD_UPPERCASE = "Password must contain at least one uppercase letter (A-Z)";
    public const string PASSWORD_LOWERCASE = "Password must contain at least one lowercase letter (a-z)";
    public const string PASSWORD_NUMBER = "Password must contain at least one number (0-9)";
    public const string PASSWORD_SPECIAL = "Password must contain at least one special character (!@#$%^&*(),.?{ }|<> )";
    public const string PASSWORD_WEAK_PATTERN = "Password contains common weak patterns";
}