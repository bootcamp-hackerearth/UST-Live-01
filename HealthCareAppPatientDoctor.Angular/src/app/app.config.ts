import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection} from '@angular/core';
import { provideRouter } from '@angular/router';

import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { provideAnimations } from '@angular/platform-browser/animations';

import { provideToastr } from 'ngx-toastr';

import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth-interceptor';

export const appConfig: ApplicationConfig = {

  providers: [

    provideBrowserGlobalErrorListeners(),


    provideRouter(routes),

    provideHttpClient(withInterceptors([authInterceptor])),

    provideAnimations(),

    provideToastr({

      positionClass:'toast-top-right',

      preventDuplicates:true,

      timeOut:3000

    })

  ]
};