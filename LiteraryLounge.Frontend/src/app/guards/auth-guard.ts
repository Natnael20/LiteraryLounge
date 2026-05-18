import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth';

/**
 * Protects routes from unauthorized access
 */
@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  /**
   * Checks if user is authenticated before allowing route access
   * @returns True if authenticated, otherwise redirects to login
   */
  canActivate(): boolean {
    // User is logged in - allow access
    if (this.authService.isLoggedIn()) {
      return true;
    }
    
    // Not logged in - redirect to login page
    this.router.navigate(['/login']);
    return false;
  }
}