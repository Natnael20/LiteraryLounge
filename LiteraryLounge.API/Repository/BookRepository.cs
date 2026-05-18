using System.Text.Json;
using LiteraryLounge.API.Models;

namespace LiteraryLounge.API.Repository;

/// <summary>
/// Handles CRUD operations for books using a JSON file as storage.
/// </summary>
/// <author>Natnael Yonas Weldetensae</author>
public class BookRepository
{
    private readonly string _filePath = Path.Combine("Data", "books.json");
    private List<Book> _books;
    private int _nextId = 1;

    /// <summary>
    /// Initializes a new repository and loads existing data from the JSON file.
    /// </summary>
    public BookRepository() 
    {
        LoadData();
    }

    /// <summary>
    /// Reads book data from the JSON file into memory.
    /// If file doesn't exist, starts with an empty collection.
    /// </summary>
    private void LoadData()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
            _nextId = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
        }
        else
        {
            _books = new List<Book>();
        }
    }

    /// <summary>
    /// Saves the current book collection to the JSON file.
    /// </summary>
    private void SaveData()
    {
        var json = JsonSerializer.Serialize(_books, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    /// <summary>
    /// Gets all books.
    /// </summary>
    /// <returns>A list of all books in the repository.</returns>
    public List<Book> GetAll()
    {
        return _books;
    }

    /// <summary>
    /// Finds a book by its unique ID.
    /// </summary>
    /// <param name="id">The book ID to search for.</param>
    /// <returns>The matching book, or null if not found.</returns>
    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    /// <summary>
    /// Adds a new book to the repository.
    /// </summary>
    /// <param name="book">The book to add (ID will be auto-generated).</param>
    /// <returns>The added book with its new ID assigned.</returns>
    public Book Create(Book book)
    {
        book.Id = _nextId++;
        _books.Add(book);
        SaveData();
        return book;
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="id">ID of the book to update.</param>
    /// <param name="book">The updated book data.</param>
    /// <returns>The updated book, or null if no book with the given ID exists.</returns>
    public Book? Update(int id, Book book)
    {
        var existing = GetById(id);
        if (existing == null)
        {
            return null;
        }

        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.ISBN = book.ISBN;
        existing.PublishYear = book.PublishYear;
        SaveData();
        return existing;
    }

    /// <summary>
    /// Removes a book from the repository.
    /// </summary>
    /// <param name="id">ID of the book to delete.</param>
    /// <returns>True if the book was deleted; false if no book with that ID was found.</returns>
    public bool Delete(int id)
    {
        var book = GetById(id);
        if (book == null)
        {
            return false;
        }

        _books.Remove(book);
        SaveData();
        return true;
    }
}