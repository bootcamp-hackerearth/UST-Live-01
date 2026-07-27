import {
  HttpContextToken,
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  Observable,
  catchError,
  finalize,
  shareReplay,
  switchMap,
  throwError
} from 'rxjs';

import {
  RefreshTokenResponse
} from '../models/auth.model';
import { AuthService } from '../services/auth.service';

const RETRY_AFTER_REFRESH =
  new HttpContextToken<boolean>(() => false);

let refreshRequest$:
  Observable<RefreshTokenResponse> | null = null;

export const authInterceptor: HttpInterceptorFn =
  (
    request,
    next
  ): Observable<HttpEvent<unknown>> => {
    const authService = inject(AuthService);
    const router = inject(Router);

    const requestWithToken = addAccessToken(
      request,
      authService.getToken()
    );

    return next(requestWithToken).pipe(
      catchError((error: unknown) => {
        if (
          !shouldTryRefresh(
            error,
            requestWithToken
          )
        ) {
          return throwError(() => error);
        }

        return refreshAndRetry(
          requestWithToken,
          next,
          authService,
          router
        );
      })
    );
  };

function addAccessToken(
  request: HttpRequest<unknown>,
  accessToken: string | null
): HttpRequest<unknown> {
  if (
    !accessToken ||
    isAuthEndpoint(request.url)
  ) {
    return request;
  }

  return addAuthorizationHeader(
    request,
    accessToken
  );
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
  return (
    error instanceof HttpErrorResponse &&
    error.status === 401 &&
    !isAuthEndpoint(request.url) &&
    !request.context.get(RETRY_AFTER_REFRESH)
  );
}

function refreshAndRetry(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService,
  router: Router
): Observable<HttpEvent<unknown>> {
  return getRefreshRequest(authService).pipe(
    catchError((refreshError: unknown) =>
      handleRefreshFailure(
        refreshError,
        authService,
        router
      )
    ),
    switchMap((response) => {
      const retryRequest =
        addAuthorizationHeader(
          request,
          response.accessToken
        ).clone({
          context: request.context.set(
            RETRY_AFTER_REFRESH,
            true
          )
        });

      return next(retryRequest);
    })
  );
}

function getRefreshRequest(
  authService: AuthService
): Observable<RefreshTokenResponse> {
  if (!refreshRequest$) {
    refreshRequest$ = authService
      .refreshToken()
      .pipe(
        finalize(() => {
          refreshRequest$ = null;
        }),
        shareReplay({
          bufferSize: 1,
          refCount: false
        })
      );
  }

  return refreshRequest$;
}

function handleRefreshFailure(
  refreshError: unknown,
  authService: AuthService,
  router: Router
): Observable<never> {
  const hadActiveSession =
    Boolean(authService.getToken());

  authService.clearSession();

  if (hadActiveSession) {
    void router.navigate(['/login'], {
      queryParams: {
        sessionExpired: 'true'
      }
    });
  }

  return throwError(() => refreshError);
}

function isAuthEndpoint(
  url: string
): boolean {
  const lowerUrl = url.toLowerCase();

  return (
    lowerUrl.includes('/auth/login') ||
    lowerUrl.includes('/auth/register') ||
    lowerUrl.includes('/auth/refresh-token')
  );
}