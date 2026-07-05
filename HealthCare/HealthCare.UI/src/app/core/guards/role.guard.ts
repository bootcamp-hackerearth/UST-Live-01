import { inject } from '@angular/core';
import {ActivatedRouteSnapshot,CanActivateFn,Router} from '@angular/router';

import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn =
(route: ActivatedRouteSnapshot) => {

  const auth = inject(AuthService);
  const router = inject(Router);

  const expectedRoles = route.data['roles'];

  const role = auth.getRole();

  if (role && expectedRoles.includes(role)) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};