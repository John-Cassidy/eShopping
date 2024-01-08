import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'store',
    loadChildren: () =>
      import('./store/store.component').then((m) => m.StoreComponent),
  },
];
