import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  UrlTree
} from '@angular/router';
import { map } from 'rxjs';

import {
  AuthService,
  UserRole
} from '../services/auth.service';

export const roleGuard: CanActivateFn = (
  route
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const hadSession =
    Boolean(authService.getToken());

  return authService.ensureAuthenticated().pipe(
    map((isAuthenticated) => {
      if (!isAuthenticated) {
        return createLoginUrlTree(
          router,
          hadSession
        );
      }

      return authorizeRole(
        route,
        authService,
        router
      );
    })
  );
};

function authorizeRole(
  route: ActivatedRouteSnapshot,
  authService: AuthService,
  router: Router
): boolean | UrlTree {
  const allowedRoles =
    getAllowedRoles(route);

  const currentRole =
    authService.role();

  if (allowedRoles.includes(currentRole)) {
    return true;
  }

  if (currentRole === 'Patient') {
    return router.createUrlTree([
      '/patient/dashboard'
    ]);
  }

  if (currentRole === 'Doctor') {
    return router.createUrlTree([
      '/doctor/dashboard'
    ]);
  }

  if (currentRole === 'Admin') {
    authService.redirectByRole();
    return false;
  }

  return router.createUrlTree(['/login']);
}

function getAllowedRoles(
  route: ActivatedRouteSnapshot
): readonly UserRole[] {
  const configuredRoles =
    route.data['roles'];

  if (!Array.isArray(configuredRoles)) {
    return [];
  }

  return configuredRoles.filter(
    (role): role is UserRole =>
      role === 'Patient' ||
      role === 'Doctor' ||
      role === 'Admin'
  );
}

function createLoginUrlTree(
  router: Router,
  sessionExpired: boolean
): UrlTree {
  if (!sessionExpired) {
    return router.createUrlTree(['/login']);
  }

  return router.createUrlTree(['/login'], {
    queryParams: {
      sessionExpired: 'true'
    }
  });
}