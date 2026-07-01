import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router
} from '@angular/router';

import { TokenService } from '../models/token.service';

export const roleGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);

  const expectedRoles =
    route.data['roles'] as string[];

  const userRole =
    tokenService.getUserRole();

  if (!tokenService.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  const normalizedUserRole =
    userRole?.toLowerCase();

  const normalizedExpectedRoles =
    expectedRoles.map(role => role.toLowerCase());

  if (
    normalizedUserRole &&
    normalizedExpectedRoles.includes(normalizedUserRole)
  ) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};