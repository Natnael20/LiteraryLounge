import { Component, OnInit } from '@angular/core';
import { ThemeService } from '../../services/theme';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css'],
  standalone: false
})
export class LandingComponent implements OnInit {
  isDarkMode = false;

  constructor(private themeService: ThemeService) {}

  ngOnInit(): void {
    this.themeService.getTheme().subscribe((theme: string) => {
      this.isDarkMode = theme === 'dark';
    });
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }
}