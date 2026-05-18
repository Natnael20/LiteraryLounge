using LiteraryLounge.API.Services;
using LiteraryLounge.API.Repository;  // ← Changed from Data to Repository
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKeyThatIsAtLeast32CharactersLong!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "LiteraryLounge";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "LiteraryLoungeUsers";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:4200",
                "https://YOUR_NETLIFY_URL.netlify.app"  // Add your Netlify URL
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddControllers();

// Register Repositories
builder.Services.AddSingleton<BookRepository>();
builder.Services.AddSingleton<QuoteRepository>();
builder.Services.AddSingleton<UserRepository>();

// Register Services
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<QuoteService>();
builder.Services.AddSingleton<AuthService>();

var app = builder.Build();

app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();