import { Routes } from '@angular/router';
import { STORE_ROUTES } from './store/store.routes';
import { StoreComponent } from './store/store.component';

export const APP_ROUTES: Routes = [
  { path: '', redirectTo: 'store', pathMatch: 'full' },
  {
    path: 'store',
    loadChildren: () =>
      import('./store/store.routes').then((m) => m.STORE_ROUTES),
  },
];
