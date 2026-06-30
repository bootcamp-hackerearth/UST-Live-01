import { HttpInterceptorFn } from '@angular/common/http';

const ACCESS_TOKEN_KEY = 'healthaxis_access_token';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);

  if (!accessToken) {
    return next(request);
  }

  const authRequest = request.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`
    }
  });

  return next(authRequest);
};