import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BookService, Book } from '../../../services/book';

/**
 * Component for adding new books to the collection
 */
@Component({
  selector: 'app-book-add',
  templateUrl: './book-add.component.html',
  styleUrls: ['./book-add.component.css'],
  standalone: false
})
export class BookAddComponent {
  // New book form model with default publish year
  newBook: Book = { title: '', author: '', isbn: '', publishYear: new Date().getFullYear() };
  
  // UI state variables
  errorMessage = '';
  successMessage = '';
  currentYear = new Date().getFullYear();

  constructor(
    private bookService: BookService,
    private router: Router
  ) { }

  /**
   * Submits the new book to the API
   * Shows success message and redirects on success
   */
  addBook(): void {
    this.bookService.createBook(this.newBook).subscribe({
      next: () => {
        this.successMessage = 'Book added successfully!';
        // Delay redirect to show success message
        setTimeout(() => {
          this.router.navigate(['/books']);
        }, 1500);
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'Error creating book';
      }
    });
  }

  /**
   * Cancels book creation and returns to book list
   */
  cancel(): void {
    this.router.navigate(['/books']);
  }
}