import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';

/**
 * Registration component for new user signup
 */
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: false
})
export class RegisterComponent {
  // Form fields
  username = '';
  password = '';
  confirmPassword = '';
  
  // UI state
  errorMessage = '';
  successMessage = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  /**
   * Handles form submission
   * Validates passwords match, then registers user
   */
  onSubmit(): void {
    // Check if passwords match
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }
    
    this.loading = true;       // Show loading indicator
    this.errorMessage = '';    // Clear previous errors
    
    // Attempt registration
    this.authService.register({ username: this.username, password: this.password }).subscribe({
      next: () => {
        this.successMessage = 'Registration successful!';
        // Delay redirect to show success message
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (err: any) => {
        this.loading = false;  // Hide loading
        this.errorMessage = err.error?.message || 'Registration failed';
      }
    });
  }
}