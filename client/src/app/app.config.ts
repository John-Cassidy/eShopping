import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { APP_ROUTES } from './app.routes';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { provideRouter } from '@angular/router';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(APP_ROUTES),
    provideHttpClient(
      // registering interceptors
      withInterceptors([errorInterceptor])
    ),
    importProvidersFrom(PaginationModule.forRoot()),
  ],
};
