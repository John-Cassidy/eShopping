import { RouterModule, RouterOutlet } from '@angular/router';

import { BreadcrumbModule } from 'xng-breadcrumb';
import { Component } from '@angular/core';
import { CoreComponent } from './core';
import { HomeComponent } from './home/home.component';
import { SharedComponent } from './shared';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CoreComponent,
    SharedComponent,
    HomeComponent,
    RouterModule,
    BreadcrumbModule,
  ],
  providers: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'eShopping';

  constructor() {
    // constructor logic here
  }

  ngOnInit() {}
}
