import {
  Injectable,
  signal
} from '@angular/core';

export type HealthAxisTheme = 'light' | 'dark';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly storageKey = 'healthaxis_theme';

  private readonly themeSignal = signal<HealthAxisTheme>(
    this.getInitialTheme()
  );

  readonly theme = this.themeSignal.asReadonly();

  constructor() {
    this.applyTheme(this.themeSignal());
  }

  toggleTheme(): void {
    const nextTheme: HealthAxisTheme =
      this.themeSignal() === 'dark'
        ? 'light'
        : 'dark';

    this.setTheme(nextTheme);
  }

  setTheme(theme: HealthAxisTheme): void {
    this.themeSignal.set(theme);

    localStorage.setItem(this.storageKey, theme);

    this.applyTheme(theme);
  }

  isDarkMode(): boolean {
    return this.themeSignal() === 'dark';
  }

  private getInitialTheme(): HealthAxisTheme {
    const savedTheme = localStorage.getItem(this.storageKey);

    if (savedTheme === 'light' || savedTheme === 'dark') {
      return savedTheme;
    }

    return 'light';
  }

  private applyTheme(theme: HealthAxisTheme): void {
    document.documentElement.dataset['theme'] = theme;
  }
}
