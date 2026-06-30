import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (request, next) => {
  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      let friendlyMessage = 'Something went wrong. Please try again.';

      if (error.status === 0) {
        friendlyMessage = 'Cannot connect to backend API. Please check if ASP.NET Core API is running.';
      }

      if (error.status === 400) {
        friendlyMessage = error.error?.message || error.error?.title || 'Invalid data. Please check your input.';
      }

      if (error.status === 401) {
        friendlyMessage = 'Your session expired. Please login again.';
      }

      if (error.status === 403) {
        friendlyMessage = 'You do not have permission to access this page.';
      }

      if (error.status === 404) {
        friendlyMessage = error.error?.message || 'Requested data was not found.';
      }

      if (error.status === 409) {
        friendlyMessage = error.error?.message || 'This appointment slot is already booked. Please choose another slot.';
      }

      if (error.status >= 500) {
        friendlyMessage = error.error?.message || 'Server error. Please contact admin.';
      }

      return throwError(() => ({
        ...error,
        friendlyMessage
      }));
    })
  );
};