import { BasketService } from '../../basket/basket.service';
import { COMMON_COMPONENTS } from '../components';
import { COMMON_DIRECTIVES } from '../directives';
import { COMMON_PIPES } from '../pipes';
import { Component } from '@angular/core';
import { SharedComponent } from '..';

@Component({
  selector: 'app-order-summary',
  standalone: true,
  imports: [COMMON_COMPONENTS, COMMON_DIRECTIVES, COMMON_PIPES],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss',
})
export class OrderSummaryComponent {
  constructor(public basketService: BasketService) {}
}
