import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, NavigationEnd } from '@angular/router';
import { filter, Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  imports: [
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements OnInit, OnDestroy {
  currentPath = '';
  private routerSubscription?: Subscription;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.setCurrentPath(this.router.url);

    this.routerSubscription = this.router.events
      .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe((event) => {
        this.setCurrentPath(event.urlAfterRedirects);
      });
  }

  ngOnDestroy(): void {
    this.routerSubscription?.unsubscribe();
  }

  private setCurrentPath(url: string): void {
    const cleanUrl = url.split('?')[0].split('#')[0];
    this.currentPath = cleanUrl === '' ? '/' : cleanUrl;
  }

  get isHomePage(): boolean {
    return this.currentPath === '/';
  }

  get isLoginPage(): boolean {
    return this.currentPath === '/login';
  }

  get isRegisterPage(): boolean {
    return this.currentPath === '/register';
  }

  get showHomeLink(): boolean {
    return !this.isHomePage;
  }

  get showPatientJourneyLink(): boolean {
    return this.isHomePage;
  }

  get showLoginLink(): boolean {
    return !this.isLoginPage;
  }

  get showRegisterLink(): boolean {
    return !this.isRegisterPage;
  }
}
