import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService, Book } from '../../../services/book';

/**
 * Component for editing existing books
 */
@Component({
  selector: 'app-book-edit',
  templateUrl: './book-edit.component.html',
  styleUrls: ['./book-edit.component.css'],
  standalone: false
})
export class BookEditComponent implements OnInit {
  // Book object to edit with default values
  book: Book = { title: '', author: '', isbn: '', publishYear: new Date().getFullYear() };
  
  // UI state variables
  errorMessage = '';
  successMessage = '';
  currentYear = new Date().getFullYear();

  constructor(
    private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  /**
   * Loads book data when component initializes
   */
  ngOnInit(): void {
    // Get book ID from URL parameter
    const id = Number(this.route.snapshot.paramMap.get('id'));
    
    // Fetch existing book data
    this.bookService.getBookById(id).subscribe({
      next: (book: Book) => {
        this.book = book;  // Populate form with existing data
      },
      error: () => {
        this.errorMessage = 'Book not found';
      }
    });
  }

  /**
   * Submits updated book data to the API
   * Shows success message and redirects on success
   */
  updateBook(): void {
    this.bookService.updateBook(this.book.id!, this.book).subscribe({
      next: () => {
        this.successMessage = 'Book updated successfully!';
        // Delay redirect to show success message
        setTimeout(() => {
          this.router.navigate(['/books']);
        }, 1500);
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'Error updating book';
      }
    });
  }

  /**
   * Cancels editing and returns to book list
   */
  cancel(): void {
    this.router.navigate(['/books']);
  }
}