import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router,
  UrlTree
} from '@angular/router';
import { map } from 'rxjs';

import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const hadSession =
    Boolean(authService.getToken());

  return authService.ensureAuthenticated().pipe(
    map((isAuthenticated) =>
      isAuthenticated
        ? true
        : createLoginUrlTree(
            router,
            hadSession
          )
    )
  );
};

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