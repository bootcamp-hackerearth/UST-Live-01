import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject
} from '@angular/core';
import {
  NavigationEnd,
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';
import {
  filter
} from 'rxjs';
import {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-public-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PublicLayout
  implements AfterViewInit {
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);

  constructor() {
    this.router.events
      .pipe(
        filter(
          (event) =>
            event instanceof NavigationEnd
        ),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(() => {
        this.scrollToCurrentFragment();
      });
  }

  ngAfterViewInit(): void {
    this.scrollToCurrentFragment();
  }

  navigateToSection(
    sectionId: string,
    event?: Event
  ): void {
    event?.preventDefault();

    const currentPath =
      this.router.url.split(/[?#]/)[0];

    if (currentPath === '/') {
      this.scrollToSection(sectionId);
      return;
    }

    void this.router.navigate(
      ['/'],
      {
        fragment: sectionId
      }
    );
  }

  private scrollToCurrentFragment(): void {
    const fragment =
      this.router.parseUrl(
        this.router.url
      ).fragment;

    if (!fragment) {
      return;
    }

    globalThis.setTimeout(() => {
      this.scrollToSection(fragment);
    });
  }

  private scrollToSection(
    sectionId: string
  ): void {
    const element =
      globalThis.document?.getElementById(
        sectionId
      );

    if (!element) {
      return;
    }

    element.scrollIntoView({
      behavior: 'smooth',
      block: 'start'
    });
  }
}