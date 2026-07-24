import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (
  request,
  next
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getAccessTokenSnapshot();

  // CHANGED:
  // These endpoints do not require an existing JWT access token.
  const isPublicAuthenticationRequest =
    request.url.includes('/api/auth/login') ||
    request.url.includes('/api/auth/register') ||
    request.url.includes('/api/auth/forgot-password') ||
    request.url.includes('/api/auth/reset-password') ||
    request.url.includes('/api/auth/refresh-token');

  // CHANGED:
  // Do not attach an old or expired JWT token to public
  // authentication requests.
  const authenticatedRequest =
    token && !isPublicAuthenticationRequest
      ? request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
      : request;

  return next(authenticatedRequest).pipe(
    catchError(error => {
      // CHANGED:
      // A 401 response from login usually means invalid credentials.
      // Do not invoke logout for public authentication requests because
      // the login component must display the backend error message.
      if (
        error.status === 401 &&
        token &&
        !isPublicAuthenticationRequest
      ) {
        authService.logout();
      }

      if (
        error.status === 403 &&
        !isPublicAuthenticationRequest
      ) {
        router.navigate(['/forbidden']);
      }

      return throwError(() => error);
    })
  );
};
