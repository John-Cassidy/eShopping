import { BasketService } from './basket/basket.service';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { Component } from '@angular/core';
import { CoreComponent } from './core';
import { HomeComponent } from './home/home.component';
import { RouterOutlet } from '@angular/router';
import { SharedComponent } from './shared';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CoreComponent,
    SharedComponent,
    HomeComponent,
    BreadcrumbModule,
  ],
  providers: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'eShopping';

  constructor(private basketService: BasketService) {}

  ngOnInit(): void {
    const basket_username = localStorage.getItem('basket_username');
    if (basket_username) {
      this.basketService.getBasket(basket_username);
    }
  }
}
