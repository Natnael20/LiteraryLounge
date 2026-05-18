import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookService, Book } from '../../../services/book';

/**
 * Component for displaying and managing all books
 */
@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css'],
  standalone: false
})
export class BookListComponent implements OnInit {
  // Collections and state
  books: Book[] = [];                    // All books from API
  selectedBook: Book | null = null;     // Currently viewed book
  showBookDetails = false;               // Modal/details visibility

  constructor(
    private bookService: BookService,
    private router: Router
  ) {}

  /**
   * Loads books when component initializes
   */
  ngOnInit(): void {
    this.loadAllBooks();
  }

  /**
   * Fetches all books from the API
   */
  loadAllBooks(): void {
    this.bookService.getAllBooks().subscribe({
      next: (data: Book[]) => {
        this.books = data;  // Store books for display
      },
      error: (err: any) => {
        console.error('Error loading books:', err);
      }
    });
  }

  /**
   * Fetches and displays details for a specific book
   * @param id - The ID of the book to view
   */
  viewBookDetails(id: number): void {
    this.bookService.getBookById(id).subscribe({
      next: (book: Book) => {
        this.selectedBook = book;    // Store for modal display
        this.showBookDetails = true;  // Show details modal
      },
      error: (err: any) => {
        console.error('Error loading book details:', err);
      }
    });
  }

  /**
   * Closes the book details modal
   */
  hideDetails(): void {
    this.showBookDetails = false;
    this.selectedBook = null;
  }

  /**
   * Navigates to the add book form
   */
  goToAddBook(): void {
    this.router.navigate(['/add-book']);
  }

  /**
   * Navigates to the edit book form
   * @param id - The ID of the book to edit
   */
  editBook(id: number): void {
    this.router.navigate(['/edit-book', id]);
  }

  /**
   * Deletes a book after user confirmation
   * @param id - The ID of the book to delete
   */
  deleteBook(id: number): void {
    if (confirm('Are you sure you want to delete this book?')) {
      this.bookService.deleteBook(id).subscribe({
        next: () => {
          this.loadAllBooks();  // Refresh the list after deletion
        },
        error: (err: any) => {
          console.error('Error deleting book:', err);
        }
      });
    }
  }
}