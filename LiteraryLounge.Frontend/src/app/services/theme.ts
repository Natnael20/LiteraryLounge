import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private theme = new BehaviorSubject<string>('light');

  constructor() {
    const savedTheme = localStorage.getItem('theme') || 'light';
    this.setTheme(savedTheme);
  }

  getTheme() {
    return this.theme.asObservable();
  }

  setTheme(theme: string): void {
    document.documentElement.setAttribute('data-bs-theme', theme);
    localStorage.setItem('theme', theme);
    this.theme.next(theme);
  }

  toggleTheme(): void {
    const currentTheme = document.documentElement.getAttribute('data-bs-theme');
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
    this.setTheme(newTheme);
  }
}