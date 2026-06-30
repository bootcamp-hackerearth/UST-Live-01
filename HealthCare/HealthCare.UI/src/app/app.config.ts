import { ApplicationConfig, provideBrowserGlobalErrorListeners }from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors }from '@angular/common/http';
import { routes } from './app.routes';
import { authInterceptor }from './core/interceptors/auth-interceptor';

import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';


export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAnimations(),
  provideToastr({
  timeOut: 3000,
  progressBar: true,
  closeButton: true,
  positionClass: 'toast-top-right'
})

   
  ]
};