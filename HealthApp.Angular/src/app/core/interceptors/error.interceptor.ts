import { inject } from '@angular/core';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

import { NotificationService } from '../services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (request, next) => {
  const notificationService = inject(NotificationService);

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      let message = 'Something went wrong. Please try again.';

      if (error.status === 0) {
        message = 'Unable to connect to the server. Please check if the API is running.';
      } else if (error.status === 400) {
        message = error.error?.message || 'Invalid request.';
      } else if (error.status === 401) {
        message = error.error?.message || 'Invalid credentials or session expired.';
      } else if (error.status === 403) {
        message = 'You are not allowed to perform this action.';
      } else if (error.status === 404) {
        message = 'Requested resource was not found.';
      } else if (error.status >= 500) {
        message = 'Server error. Please try again later.';
      }

      notificationService.error(message);

      return throwError(() => error);
    })
  );
};