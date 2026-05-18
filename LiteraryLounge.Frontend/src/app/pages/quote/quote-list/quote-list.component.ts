import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { QuoteService, Quote } from '../../../services/quote';

/**
 * Component for displaying and managing all quotes
 */
@Component({
  selector: 'app-quote-list',
  templateUrl: './quote-list.component.html',
  styleUrls: ['./quote-list.component.css'],
  standalone: false
})
export class QuoteListComponent implements OnInit {
  // Collections and state
  quotes: Quote[] = [];                    // All quotes from API
  selectedQuote: Quote | null = null;     // Currently viewed quote
  showQuoteDetails = false;                // Modal/details visibility

  constructor(
    private quoteService: QuoteService,
    private router: Router
  ) { }

  /**
   * Loads quotes when component initializes
   */
  ngOnInit(): void {
    this.loadAllQuotes();
  }

  /**
   * Fetches all quotes from the API
   */
  loadAllQuotes(): void {
    this.quoteService.getAllQuotes().subscribe({
      next: (data: Quote[]) => {
        this.quotes = data;  // Store quotes for display
      },
      error: (err: any) => {
        console.error('Error loading quotes:', err);
      }
    });
  }

  /**
   * Fetches and displays details for a specific quote
   * @param id - The ID of the quote to view
   */
  viewQuoteDetails(id: number): void {
    this.quoteService.getQuoteById(id).subscribe({
      next: (quote: Quote) => {
        this.selectedQuote = quote;    // Store for modal display
        this.showQuoteDetails = true;   // Show details modal
      },
      error: (err: any) => {
        console.error('Error loading quote details:', err);
      }
    });
  }

  /**
   * Closes the quote details modal
   */
  hideDetails(): void {
    this.showQuoteDetails = false;
    this.selectedQuote = null;
  }

  /**
   * Navigates to the add quote form
   */
  goToAddQuote(): void {
    this.router.navigate(['/add-quote']);
  }

  /**
   * Navigates to the edit quote form
   * @param id - The ID of the quote to edit
   */
  editQuote(id: number): void {
    this.router.navigate(['/edit-quote', id]);
  }

  /**
   * Deletes a quote after user confirmation
   * @param id - The ID of the quote to delete
   */
  deleteQuote(id: number): void {
    if (confirm('Are you sure you want to delete this quote?')) {
      this.quoteService.deleteQuote(id).subscribe({
        next: () => {
          this.loadAllQuotes();  // Refresh the list after deletion
        },
        error: (err: any) => {
          console.error('Error deleting quote:', err);
        }
      });
    }
  }
}