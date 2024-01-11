import { BasketService } from '../../basket/basket.service';
import { Component } from '@angular/core';
import { IBasketItem } from '../../shared/models/basket';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [SharedComponent],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  constructor(public basketService: BasketService) {}

  getBasketCount(items: IBasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }
}
