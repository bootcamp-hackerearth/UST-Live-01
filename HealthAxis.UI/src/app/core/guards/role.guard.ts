import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';

import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.ensureAuthenticated().pipe(
    map((isAuthenticated) => {
      if (!isAuthenticated) {
        return router.createUrlTree(['/login'], {
          queryParams: {
            sessionExpired: 'true'
          }
        });
      }

      const allowedRoles = route.data['roles'] as string[];
      const currentRole = authService.role();

      if (allowedRoles.includes(currentRole)) {
        return true;
      }

      if (currentRole === 'Patient') {
        return router.createUrlTree(['/patient/dashboard']);
      }

      if (currentRole === 'Doctor') {
        return router.createUrlTree(['/doctor/dashboard']);
      }

      if (currentRole === 'Admin') {
        authService.redirectByRole();
        return false;
      }

      return router.createUrlTree(['/login']);
    })
  );
};