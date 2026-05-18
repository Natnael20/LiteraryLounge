import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';
import { ThemeService } from '../../services/theme';

/**
 * Login component for user authentication
 */
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false
})
export class LoginComponent {
  // Form fields
  username = '';
  password = '';
  
  // UI state
  errorMessage = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  /**
   * Handles form submission
   * Authenticates user and redirects to books page on success
   */
  onSubmit(): void {
    this.loading = true;
    this.errorMessage = '';
    
    // Attempt login
    this.authService.login({ username: this.username, password: this.password }).subscribe({
      next: () => {
        this.loading = false;                     // Hide loading
        this.router.navigate(['/books'], { replaceUrl: true }); // Redirect to books
      },
      error: (err: any) => {
        this.loading = false;                     // Hide loading
        this.errorMessage = err.error?.message || 'Login failed'; // Show error
      }
    });
  }
}