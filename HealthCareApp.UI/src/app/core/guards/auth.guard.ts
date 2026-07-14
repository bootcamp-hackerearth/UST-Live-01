import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router
} from '@angular/router';

import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true;
  }

  const shouldShowSessionExpiredMessage =
    authService.hasStoredSession() || authService.isTokenExpired();

  authService.logout();

  if (shouldShowSessionExpiredMessage) {
    return router.createUrlTree(['/'], {
      queryParams: {
        sessionExpired: 'true'
      }
    });
  }

  return router.createUrlTree(['/']);
};