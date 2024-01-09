import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CoreComponent } from '../core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/products';
import { IType } from '../shared/models/type';
import { ProductItemsComponent } from './product-items/product-items.component';
import { SharedCompnent } from '../shared';
import { StoreParams } from '../shared/models/storeParams';
import { StoreService } from './store.service';

@Component({
  selector: 'app-store',
  standalone: true,
  imports: [CommonModule, CoreComponent, SharedCompnent, ProductItemsComponent],
  templateUrl: './store.component.html',
  styleUrl: './store.component.scss',
})
export class StoreComponent {
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];
  storeParams = new StoreParams();
  totalCount = 0;

  constructor(private storeService: StoreService) {}

  ngOnInit() {
    this.getProducts();
    this.GetBrands();
    this.GetTypes();
  }

  private getProducts() {
    this.storeService.getProducts(this.storeParams).subscribe({
      next: (response) => {
        this.products = response.data;
        this.storeParams.pageNumber = response.pageIndex;
        this.storeParams.pageSize = response.pageSize;
        this.totalCount = response.count;
        console.log(this.products);
      },
      error: (err) => console.log(err),
      complete: () => console.log('Catalog API call to GetProducts completed'),
    });
  }
  GetBrands() {
    this.storeService.getBrands().subscribe({
      next: (response) => {
        this.brands = [{ id: '', name: 'All' }, ...response];
      },
      error: (err) => console.log(err),
      complete: () => console.log('Catalog API call to GetBrands completed'),
    });
  }
  GetTypes() {
    this.storeService.getTypes().subscribe({
      next: (response) => {
        this.types = [{ id: '', name: 'All' }, ...response];
      },
      error: (err) => console.log(err),
      complete: () => console.log('Catalog API call to GetTypes completed'),
    });
  }
  onBrandSelected(brandId: string) {
    this.storeParams.brandId = brandId;
    // this.storeParams.pageNumber = 1;
    this.getProducts();
  }
  onTypeSelected(typeId: string) {
    this.storeParams.typeId = typeId;
    // this.storeParams.pageNumber = 1;
    this.getProducts();
  }
}
