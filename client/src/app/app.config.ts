import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { APP_ROUTES } from './app.routes';
import { LoadingService } from './core/services/loading.service';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { loadingInterceptor } from './core/interceptors/loading.interceptor';
import { provideRouter } from '@angular/router';

export const appConfig: ApplicationConfig = {
  providers: [
    LoadingService,
    provideRouter(APP_ROUTES),
    provideHttpClient(
      // registering interceptors
      withInterceptors([errorInterceptor, loadingInterceptor])
    ),
    importProvidersFrom(PaginationModule.forRoot()),
  ],
};
