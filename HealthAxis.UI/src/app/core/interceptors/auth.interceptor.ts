import {
  HttpErrorResponse,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  BehaviorSubject,
  catchError,
  filter,
  switchMap,
  take,
  throwError
} from 'rxjs';

import { AuthService } from '../services/auth.service';

const ACCESS_TOKEN_KEY = 'healthaxis_access_token';
const RETRY_AFTER_REFRESH_HEADER = 'X-Retry-After-Refresh';

let isRefreshingToken = false;

const refreshedTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const requestWithToken = addAccessToken(request);

  return next(requestWithToken).pipe(
    catchError((error: unknown) => {
      if (!shouldTryRefresh(error, requestWithToken)) {
        return throwError(() => error);
      }

      return refreshTokenAndRetryRequest(
        requestWithToken,
        next,
        authService,
        router
      );
    })
  );
};

function addAccessToken(request: HttpRequest<unknown>): HttpRequest<unknown> {
  const accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);

  if (!accessToken || isAuthEndpoint(request.url)) {
    return request;
  }

  return addAuthorizationHeader(request, accessToken);
}

function addAuthorizationHeader(
  request: HttpRequest<unknown>,
  accessToken: string
): HttpRequest<unknown> {
  return request.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`
    }
  });
}

function shouldTryRefresh(
  error: unknown,
  request: HttpRequest<unknown>
): boolean {
  if (!(error instanceof HttpErrorResponse)) {
    return false;
  }

  if (error.status !== 401) {
    return false;
  }

  if (isAuthEndpoint(request.url)) {
    return false;
  }

  return !request.headers.has(RETRY_AFTER_REFRESH_HEADER);
}

function refreshTokenAndRetryRequest(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService,
  router: Router
) {
  if (isRefreshingToken) {
    return waitForRefreshAndRetry(request, next);
  }

  isRefreshingToken = true;
  refreshedTokenSubject.next(null);

  return authService.refreshToken().pipe(
    switchMap((response) => {
      isRefreshingToken = false;
      refreshedTokenSubject.next(response.accessToken);

      const retryRequest = request.clone({
        setHeaders: {
          Authorization: `Bearer ${response.accessToken}`,
          [RETRY_AFTER_REFRESH_HEADER]: 'true'
        }
      });

      return next(retryRequest);
    }),
    catchError((refreshError: unknown) => {
      isRefreshingToken = false;
      refreshedTokenSubject.next(null);

      authService.clearSession();

      void router.navigate(['/login'], {
        queryParams: {
          sessionExpired: 'true'
        }
      });

      return throwError(() => refreshError);
    })
  );
}

function waitForRefreshAndRetry(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn
) {
  return refreshedTokenSubject.pipe(
    filter(Boolean),
    take(1),
    switchMap((newAccessToken) => {
      const retryRequest = request.clone({
        setHeaders: {
          Authorization: `Bearer ${newAccessToken}`,
          [RETRY_AFTER_REFRESH_HEADER]: 'true'
        }
      });

      return next(retryRequest);
    })
  );
}

function isAuthEndpoint(url: string): boolean {
  const lowerUrl = url.toLowerCase();

  return (
    lowerUrl.includes('/auth/login') ||
    lowerUrl.includes('/auth/register') ||
    lowerUrl.includes('/auth/refresh-token')
  );
}