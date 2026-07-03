import { inject } from '@angular/core';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { NotificationService } from '../services/notification.service';
import { AuthService } from '../services/auth.service';

interface ApiErrorResponse {
  statusCode?: number;
  StatusCode?: number;
  message?: string;
  Message?: string;
  details?: string | null;
  Details?: string | null;
  timeStamp?: string;
  TimeStamp?: string;
  path?: string;
  Path?: string;
  title?: string;
  errors?: unknown;
}

export const errorInterceptor: HttpInterceptorFn = (request, next) => {
  const notificationService = inject(NotificationService);
  const authService = inject(AuthService);
  const router = inject(Router);

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      const message = getErrorMessage(error);

      if (shouldShowToast(request.url, error.status)) {
        notificationService.error(message);
      }

      if (error.status === 401 && !isLoginRequest(request.url)) {
        authService.logout();
        router.navigate(['/login']);
      }

      return throwError(() => error);
    })
  );
};

function getErrorMessage(error: HttpErrorResponse): string {
  if (error.status === 0) {
    return 'Unable to connect to the server. Please check if the API is running.';
  }

  const backendMessage = extractBackendMessage(error.error);

  if (backendMessage) {
    return backendMessage;
  }

  switch (error.status) {
    case 400:
      return 'Invalid request.';

    case 401:
      return 'Invalid credentials or session expired.';

    case 403:
      return 'You are not allowed to perform this action.';

    case 404:
      return 'Requested resource was not found.';

    case 409:
      return 'Conflict occurred. Please refresh and try again.';

    case 422:
      return 'Validation failed. Please check the entered details.';

    case 500:
      return 'Server error. Please try again later.';

    default:
      if (error.status >= 500) {
        return 'Server error. Please try again later.';
      }

      return 'Something went wrong. Please try again.';
  }
}

function extractBackendMessage(errorBody: unknown): string | null {
  if (!errorBody) {
    return null;
  }

  if (typeof errorBody === 'string') {
    return errorBody.trim() || null;
  }

  if (typeof errorBody !== 'object') {
    return null;
  }

  const apiError = errorBody as ApiErrorResponse;

  const message = apiError.message || apiError.Message;
  if (typeof message === 'string' && message.trim()) {
    return message.trim();
  }

  const title = apiError.title;
  if (typeof title === 'string' && title.trim()) {
    return title.trim();
  }

  const validationMessage = extractValidationErrors(apiError.errors);
  if (validationMessage) {
    return validationMessage;
  }

  const details = apiError.details || apiError.Details;
  if (typeof details === 'string' && details.trim()) {
    return details.trim();
  }

  return null;
}

function extractValidationErrors(errors: unknown): string | null {
  if (!errors) {
    return null;
  }

  if (Array.isArray(errors)) {
    const messages = errors
      .filter((item): item is string => typeof item === 'string')
      .map(item => item.trim())
      .filter(Boolean);

    return messages.length > 0 ? messages.join('\n') : null;
  }

  if (typeof errors === 'object') {
    const messages: string[] = [];

    Object.values(errors as Record<string, unknown>).forEach(value => {
      if (Array.isArray(value)) {
        value.forEach(item => {
          if (typeof item === 'string' && item.trim()) {
            messages.push(item.trim());
          }
        });
      } else if (typeof value === 'string' && value.trim()) {
        messages.push(value.trim());
      }
    });

    return messages.length > 0 ? messages.join('\n') : null;
  }

  return null;
}

function shouldShowToast(url: string, status: number): boolean {
  return true;
}

function isLoginRequest(url: string): boolean {
  return url.toLowerCase().includes('/auth/login');
}