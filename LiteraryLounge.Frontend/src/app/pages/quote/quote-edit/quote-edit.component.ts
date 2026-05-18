import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuoteService, Quote } from '../../../services/quote';

@Component({
  selector: 'app-quote-edit',
  templateUrl: './quote-edit.component.html',
  styleUrls: ['./quote-edit.component.css'],
  standalone: false
})
export class QuoteEditComponent implements OnInit {
  quote: Quote = { text: '', author: '', source: '' };
  errorMessage = '';
  successMessage = '';

  constructor(
    private quoteService: QuoteService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.quoteService.getQuoteById(id).subscribe({
      next: (quote: Quote) => {
        this.quote = quote;
      },
      error: () => {
        this.errorMessage = 'Quote not found';
      }
    });
  }

  updateQuote(): void {
    this.quoteService.updateQuote(this.quote.id!, this.quote).subscribe({
      next: () => {
        this.successMessage = 'Quote updated successfully!';
        setTimeout(() => {
          this.router.navigate(['/quotes']);
        }, 1500);
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'Error updating quote';
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/quotes']);
  }
}