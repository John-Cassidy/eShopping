import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { BasketService } from '../../basket/basket.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { CommonModule } from '@angular/common';
import { IProduct } from '../../shared/models/products';
import { SharedComponent } from '../../shared';
import { StoreService } from '../store.service';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, SharedComponent],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss',
})
export class ProductDetailsComponent implements OnInit {
  product?: IProduct;
  quantity = 1;

  constructor(
    private storeService: StoreService,
    private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private basketService: BasketService
  ) {}

  ngOnInit(): void {
    this.loadProduct();
  }
  loadProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.storeService.getProductById(id).subscribe({
        next: (response) => {
          this.product = response;
          this.bcService.set('@productDetails', response.name);
        },
        error: (error) => console.log(error),
      });
    }
  }
  addItemToCart() {
    if (this.product) {
      this.basketService.addItemToBasket(this.product, this.quantity);
    }
  }
  incrementQuantity() {
    this.quantity++;
  }
  decrementQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }
}
