import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { TokenService } from '../services/token.service';

export const roleGuard: CanActivateFn = (route) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);

  const allowedRoles = route.data?.['roles'] as string[] | undefined;
  const currentRole = tokenService.getRole();

  if (!allowedRoles || !allowedRoles.length) {
    return true;
  }

  if (!currentRole) {
    router.navigate(['/login']);
    return false;
  }

  const hasAccess = allowedRoles
    .map((role) => role.toLowerCase())
    .includes(currentRole.toLowerCase());

  if (hasAccess) {
    return true;
  }

  router.navigate(['/']);
  return false;
};


