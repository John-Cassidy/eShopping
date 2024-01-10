import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
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
    private activatedRoute: ActivatedRoute
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
        },
        error: (error) => console.log(error),
      });
    }
  }
}
