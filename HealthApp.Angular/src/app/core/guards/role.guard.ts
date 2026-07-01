import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { UserRole } from '../models/current-user';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = route => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const allowedRoles = route.data['roles'] as UserRole[] | undefined;
  const currentRole = authService.currentRole();

  if (!allowedRoles || allowedRoles.length === 0) {
    return true;
  }

  if (currentRole && allowedRoles.includes(currentRole)) {
    return true;
  }

  return router.parseUrl('/login');
};