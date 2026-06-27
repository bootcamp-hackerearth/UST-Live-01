import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

    const token = localStorage.getItem('token');

  // Skip adding token for login/register requests
  if (
    req.url.includes('/login') ||
    req.url.includes('/register')
  ) {
    return next(req);
  }

  if (token) {

    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    return next(clonedRequest);
  }
  return next(req);
};
