import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from '../../services/auth';
import { ThemeService } from '../../services/theme';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  standalone: false
})
export class NavbarComponent {
  isLoggedIn = false;
  isDarkMode = false;
  isLandingPage = false;

  constructor(
    private authService: AuthService,
    private themeService: ThemeService,
    private router: Router
  ) {
    this.authService.getAuthStatus().subscribe(status => {
      this.isLoggedIn = status;
    });
    
    this.themeService.getTheme().subscribe(theme => {
      this.isDarkMode = theme === 'dark';
    });

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLandingPage = event.url === '/' || event.url === '';
        this.isLoggedIn = this.authService.isLoggedIn();
      }
    });
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }

  onBrandClick(): void {
    if (this.isLoggedIn) {
      const confirmLogout = confirm('Do you want to log out?');
      if (confirmLogout) {
        this.logout();
      }
    } else {
      this.router.navigate(['/']);
    }
  }

  isLoginOrRegisterPage(): boolean {
    const url = this.router.url;
    return url === '/login' || url === '/register';
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}