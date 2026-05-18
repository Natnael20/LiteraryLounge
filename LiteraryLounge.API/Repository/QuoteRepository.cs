using System.Text.Json;
using LiteraryLounge.API.Models;

namespace LiteraryLounge.API.Repository;

/// <summary>
/// Handles CRUD operations for quotes using a JSON file as storage.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class QuoteRepository
{
    private readonly string _filePath = Path.Combine("Data", "quotes.json");
    private List<Quote> _quotes;
    private int _nextId = 1;

    /// <summary>
    /// Initializes a new repository and loads existing data from the JSON file.
    /// </summary>
    public QuoteRepository()
    {
        LoadData();
    }

    /// <summary>
    /// Reads quote data from the JSON file into memory.
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
            _quotes = JsonSerializer.Deserialize<List<Quote>>(json) ?? new List<Quote>();
            _nextId = _quotes.Any() ? _quotes.Max(q => q.Id) + 1 : 1;
        }
        else
        {
            _quotes = new List<Quote>();
        }
    }

    /// <summary>
    /// Saves the current quote collection to the JSON file.
    /// </summary>
    private void SaveData()
    {
        var json = JsonSerializer.Serialize(_quotes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    /// <summary>
    /// Gets all quotes.
    /// </summary>
    /// <returns>A list of all quotes in the repository.</returns>
    public List<Quote> GetAll() 
    {
        return _quotes;
    }

    /// <summary>
    /// Finds a quote by its unique ID.
    /// </summary>
    /// <param name="id">The quote ID to search for.</param>
    /// <returns>The matching quote, or null if not found.</returns>
    public Quote? GetById(int id) 
    {
        return _quotes.FirstOrDefault(q => q.Id == id);
    }

    /// <summary>
    /// Adds a new quote to the repository.
    /// </summary>
    /// <param name="quote">The quote to add (ID will be auto-generated).</param>
    /// <returns>The added quote with its new ID assigned.</returns>
    public Quote Create(Quote quote)
    {
        quote.Id = _nextId++;
        _quotes.Add(quote);
        SaveData();
        return quote;
    }

    /// <summary>
    /// Updates an existing quote.
    /// </summary>
    /// <param name="id">ID of the quote to update.</param>
    /// <param name="quote">The updated quote data.</param>
    /// <returns>The updated quote, or null if no quote with the given ID exists.</returns>
    public Quote? Update(int id, Quote quote)
    {
        var existing = GetById(id);
        if (existing == null) 
        {
            return null;
        }
        
        existing.Text = quote.Text;
        existing.Author = quote.Author;
        existing.Source = quote.Source;
        SaveData();
        return existing;
    }

    /// <summary>
    /// Removes a quote from the repository.
    /// </summary>
    /// <param name="id">ID of the quote to delete.</param>
    /// <returns>True if the quote was deleted; false if no quote with that ID was found.</returns>
    public bool Delete(int id)
    {
        var quote = GetById(id);
        if (quote == null) 
        {
            return false;
        }
        
        _quotes.Remove(quote);
        SaveData();
        return true;
    }
}