import { Component, Input } from '@angular/core';

import { IProduct } from '../../shared/models/products';
import { RouterModule } from '@angular/router';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-product-items',
  standalone: true,
  imports: [RouterModule, SharedComponent],
  templateUrl: './product-items.component.html',
  styleUrl: './product-items.component.scss',
})
export class ProductItemsComponent {
  @Input() product?: IProduct;

  constructor() {}
}
