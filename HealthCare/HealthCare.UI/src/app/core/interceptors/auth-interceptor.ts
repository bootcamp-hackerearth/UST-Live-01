import {
  HttpInterceptorFn,
  HttpErrorResponse
} from '@angular/common/http';

import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  const token = authService.getToken();

  // Skip Login & Register
  if (
    req.url.includes('/login') ||
    req.url.includes('/register')
  ) {
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

      if (error.status === 401) {

        authService.logout();

        toastr.error(
          'Session expired. Please login again.',
          'Session Expired'
        );

        router.navigate(['/']);

      }

      return throwError(() => error);

    })

  );

};