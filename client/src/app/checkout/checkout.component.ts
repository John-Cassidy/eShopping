import { IBasket, IBasketItem } from '../shared/models/basket';

import { AcntService } from '../account/acnt.service';
import { BasketService } from '../basket/basket.service';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SharedComponent } from '../shared';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, SharedComponent],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent {
  constructor(
    public basketService: BasketService,
    private acntService: AcntService
  ) {}

  ngOnInit(): void {
    this.acntService.currentUser$.subscribe({
      next: (res) => {
        this.isUserAuthenticated = res;
        console.log(this.isUserAuthenticated);
      },
      error: (err) => {
        console.log(
          `An error occurred while setting isUserAuthenticated flag.`
        );
      },
    });
  }
  public isUserAuthenticated: boolean = false;

  removeBasketItem(item: IBasketItem) {
    this.basketService.removeItemFromBasket(item);
  }

  incrementItem(item: IBasketItem) {
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: IBasketItem) {
    this.basketService.decrementItemQuantity(item);
  }

  orderNow(item: IBasket) {
    this.basketService.checkoutBasket(item);
  }
}
