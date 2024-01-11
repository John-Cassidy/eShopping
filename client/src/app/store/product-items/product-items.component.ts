import { Component, Input } from '@angular/core';

import { BasketService } from '../../basket/basket.service';
import { IProduct } from '../../shared/models/products';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-product-items',
  standalone: true,
  imports: [SharedComponent],
  templateUrl: './product-items.component.html',
  styleUrl: './product-items.component.scss',
})
export class ProductItemsComponent {
  @Input() product?: IProduct;

  constructor(private basketService: BasketService) {}

  addItemToBasket() {
    this.product && this.basketService.addItemToBasket(this.product);
  }
}
