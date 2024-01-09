import { Component, Input } from '@angular/core';

import { CommonModule } from '@angular/common';
import { CoreComponent } from '../../core';
import { IProduct } from '../../shared/models/products';
import { SharedCompnent } from '../../shared';

@Component({
  selector: 'app-product-items',
  standalone: true,
  imports: [CommonModule, CoreComponent, SharedCompnent],
  templateUrl: './product-items.component.html',
  styleUrl: './product-items.component.scss',
})
export class ProductItemsComponent {
  @Input() product?: IProduct;

  constructor() {}
}
