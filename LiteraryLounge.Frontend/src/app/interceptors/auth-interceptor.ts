import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth';
import { Router } from '@angular/router';

/**
 * Intercepts HTTP requests to add authentication token
 * Handles unauthorized responses by logging out user
 */
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  /**
   * Adds JWT token to request headers and handles 401 errors
   * @param req - The outgoing HTTP request
   * @param next - The next interceptor in the chain
   * @returns Observable of the HTTP event stream
   */
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Get stored token
    const token = this.authService.getToken();
    
    // Clone request and add authorization header if token exists
    let authReq = req;
    if (token) {
      authReq = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
    }
    
    // Process request and handle errors
    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        // Token expired or invalid - logout and redirect
        if (error.status === 401) {
          this.authService.logout();
          this.router.navigate(['/login']);
        }
        return throwError(() => error);
      })
    );
  }
}