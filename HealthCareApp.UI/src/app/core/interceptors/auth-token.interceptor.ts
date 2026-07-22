import {
  HttpErrorResponse,
  HttpInterceptorFn
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  catchError,
  throwError
} from 'rxjs';

import { AuthService } from '../services/auth.service';

export const authTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getToken();

  const isApiRequest =
    req.url.startsWith('/api/') ||
    req.url.includes('/api/');

  const isAuthEndpoint =
    req.url.includes('/api/Auth/login') ||
    req.url.includes('/api/Auth/register-patient');

  if (!isApiRequest || isAuthEndpoint) {
    return next(req);
  }

  if (token && authService.isTokenExpired()) {
    authService.logout();
    redirectToSessionExpired(router);

    return throwError(() => new HttpErrorResponse({
      status: 401,
      statusText: 'Session expired',
      url: req.url,
      error: {
        message: 'Session expired. Please login again.'
      }
    }));
  }

  const authRequest = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    : req;

  return next(authRequest).pipe(
    catchError((error: unknown) => {
      if (
        error instanceof HttpErrorResponse &&
        error.status === 401
      ) {
        authService.logout();
        redirectToSessionExpired(router);
      }

      return throwError(() => error);
    })
  );
};

function redirectToSessionExpired(router: Router): void {
  router.navigate(['/'], {
    queryParams: {
      sessionExpired: 'true'
    },
    replaceUrl: true
  });
}