import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router
} from '@angular/router';

import { AuthService } from '../services/auth.service';
import { UserRole } from '../../shared/models/auth.models';

export const roleGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const allowedRoles = (route.data['roles'] ?? []) as UserRole[];
  const currentRole = authService.getRole();

  if (!authService.isLoggedIn()) {
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
  }

  if (allowedRoles.includes(currentRole as UserRole)) {
    return true;
  }

  return router.createUrlTree(['/access-denied']);
};