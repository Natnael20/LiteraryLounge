using System.Text.Json;
using LiteraryLounge.API.Models;

namespace LiteraryLounge.API.Repository;

/// <summary>
/// Handles CRUD operations for users using a JSON file as storage.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class UserRepository
{
    private readonly string _filePath = Path.Combine("Data", "users.json");
    private List<User> _users;
    private int _nextId = 1;

    /// <summary>
    /// Initializes a new repository and loads existing data from the JSON file.
    /// </summary>
    public UserRepository()
    {
        LoadData();
    }

    /// <summary>
    /// Reads user data from the JSON file into memory.
    /// If file doesn't exist, starts with an empty collection.
    /// </summary>
    private void LoadData()
    {
        // Ensure Data directory exists
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            
            if (_users.Any())
            {
                _nextId = _users.Max(u => u.Id) + 1;
            }
            // _nextId stays as 1 if no users exist
        }
        else
        {
            _users = new List<User>();
            // _nextId remains 1 (already set at field level)
        }
    }

    /// <summary>
    /// Saves the current user collection to the JSON file.
    /// </summary>
    private void SaveData()
    {
        var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A list of all users in the repository.</returns>
    public List<User> GetAll()
    {
        return _users;
    }

    /// <summary>
    /// Finds a user by their unique ID.
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>The matching user, or null if not found.</returns>
    public User? GetById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    /// <summary>
    /// Finds a user by their username (case-insensitive).
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The matching user, or null if not found.</returns>
    public User? GetByUsername(string username)
    {
        return _users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
    }

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user to add (ID will be auto-generated).</param>
    /// <returns>The added user with its new ID assigned.</returns>
    public User Create(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        SaveData();
        return user;
    }

    /// <summary>
    /// Removes a user from the repository.
    /// </summary>
    /// <param name="id">ID of the user to delete.</param>
    /// <returns>True if the user was deleted; false if no user with that ID was found.</returns>
    public bool Delete(int id)
    {
        var user = GetById(id);
        if (user == null)
        {
            return false;
        }
        
        _users.Remove(user);
        SaveData();
        return true;
    }
}