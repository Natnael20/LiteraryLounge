import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { QuoteService, Quote } from '../../../services/quote';

/**
 * Component for adding new quotes to the collection
 */
@Component({
  selector: 'app-quote-add',
  templateUrl: './quote-add.component.html',
  styleUrls: ['./quote-add.component.css'],
  standalone: false
})
export class QuoteAddComponent {
  // New quote form model
  newQuote: Quote = { text: '', author: '', source: '' };
  
  // UI state variables
  errorMessage = '';
  successMessage = '';

  constructor(
    private quoteService: QuoteService,
    private router: Router
  ) { }

  /**
   * Submits the new quote to the API
   * Shows success message and redirects on success
   */
  addQuote(): void {
    this.quoteService.createQuote(this.newQuote).subscribe({
      next: () => {
        this.successMessage = 'Quote added successfully!';
        // Delay redirect to show success message
        setTimeout(() => {
          this.router.navigate(['/quotes']);
        }, 1500);
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'Error creating quote';
      }
    });
  }

  /**
   * Cancels quote creation and returns to quote list
   */
  cancel(): void {
    this.router.navigate(['/quotes']);
  }
}