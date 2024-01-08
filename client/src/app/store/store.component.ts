import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CoreComponent } from '../core';
import { IProduct } from '../shared/models/products';
import { SharedCompnent } from '../shared';
import { StoreService } from './store.service';

@Component({
  selector: 'app-store',
  standalone: true,
  imports: [CommonModule, CoreComponent, SharedCompnent],
  templateUrl: './store.component.html',
  styleUrl: './store.component.scss',
})
export class StoreComponent {
  products: IProduct[] = [];

  constructor(private storeService: StoreService) {}

  ngOnInit() {
    this.getProducts();
  }

  private getProducts() {
    this.storeService.getProducts().subscribe({
      next: (response) => {
        this.products = response.data;
        console.log(this.products);
      },
      error: (err) => console.log(err),
      complete: () => console.log('Catalog API call completed'),
    });
  }
}
