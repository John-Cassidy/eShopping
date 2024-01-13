import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { Routes } from '@angular/router';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { SigninRedirectCallbackComponent } from './account/signin-redirect-callback/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './account/signout-redirect-callback.component/signout-redirect-callback.component.component';
import { UnauthenticatedComponent } from './core/unauthenticated/unauthenticated.component';
import { authGuardFactory } from './core/guards/auth.guard';

export const APP_ROUTES: Routes = [
  { path: '', component: HomeComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'unauthenticated', component: UnauthenticatedComponent },
  { path: 'server-error', component: ServerErrorComponent },
  {
    path: 'store',
    loadChildren: () =>
      import('./store/store.routes').then((m) => m.STORE_ROUTES),
    data: { breadcrumb: 'Store' },
  },
  { path: 'signin-callback', component: SigninRedirectCallbackComponent },
  { path: 'signout-callback', component: SignoutRedirectCallbackComponent },
  {
    path: 'basket',
    loadChildren: () =>
      import('./basket/basket.routes').then((m) => m.BASKET_ROUTES),
    data: { breadcrumb: 'Basket' },
  },
  {
    path: 'checkout',
    canActivate: [authGuardFactory],
    loadChildren: () =>
      import('./checkout/checkout.routes').then((m) => m.CHECKOUT_ROUTES),
    data: { breadcrumb: 'Checkout' },
  },
  {
    path: 'account',
    loadChildren: () =>
      import('./account/account.routes').then((m) => m.ACCOUNT_ROUTES),
    data: { breadcrumb: { skip: true } },
  },

  { path: '**', redirectTo: '', pathMatch: 'full' },
];
