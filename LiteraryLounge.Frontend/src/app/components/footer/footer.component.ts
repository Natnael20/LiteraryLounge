import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ThemeService } from '../../services/theme';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
  standalone: false
})
export class FooterComponent implements OnInit {
  isDarkMode = false;

  constructor(
    private themeService: ThemeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.themeService.getTheme().subscribe(theme => {
      this.isDarkMode = theme === 'dark';
    });
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }
}