import { BasketService } from './basket.service';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { IBasketItem } from '../shared/models/basket';
import { SharedComponent } from '../shared';

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [CommonModule, SharedComponent],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss',
})
export class BasketComponent {
  constructor(public basketService: BasketService) {}

  incrementItem(item: IBasketItem) {
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: IBasketItem) {
    this.basketService.decrementItemQuantity(item);
  }

  removeBasketItem(item: IBasketItem) {
    this.basketService.removeItemFromBasket(item);
  }
}
