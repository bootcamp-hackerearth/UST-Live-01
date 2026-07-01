import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection
} from '@angular/core';

import { provideRouter } from '@angular/router';

import { provideHttpClient, withInterceptors } from '@angular/common/http'; // ✅ updated

import { routes } from './app.routes';

// ✅ import interceptor
import { authInterceptor } from './auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [

    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),

    provideRouter(routes),

    // ✅ ✅ IMPORTANT CHANGE HERE
    provideHttpClient(
      withInterceptors([authInterceptor]) // ✅ attach interceptor
    )

  ]
};