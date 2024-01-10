import { HeaderComponent } from './header/header.component';
import { NavbarComponent } from './navbar/navbar.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { UnauthenticatedComponent } from './unauthenticated/unauthenticated.component';

export const CoreComponent = [
  HeaderComponent,
  NavbarComponent,
  NotFoundComponent,
  UnauthenticatedComponent,
  ServerErrorComponent,
  NgxSpinnerModule,
];
