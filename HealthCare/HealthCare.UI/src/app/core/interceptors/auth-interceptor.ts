import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);
  const token = authService.getToken();

  // Skip auth endpoints
  if (req.url.includes('/login') || req.url.includes('/register')) {
    return next(req);
  }

  const authReq = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {

      //  Token expired or unauthorized
      if (error.status === 401) {

        return authService.refreshToken().pipe(
          switchMap((newToken: string) => {

            // Save new token
            localStorage.setItem('token', newToken);

            // Retry original request with new token
            const retryReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`
              }
            });

            return next(retryReq);
          }),
          catchError(err => {
            //    Token expired
            alert('Session expired. Please login again.');
            authService.logout();
            return throwError(() => err);
          })
        );
      }

      return throwError(() => error);
    })
  );
};