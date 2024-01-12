import { COMMON_COMPONENTS } from './components';
import { COMMON_DIRECTIVES } from './directives';
import { COMMON_PIPES } from './pipes';
import { OrderSummaryComponent } from './order-summary/order-summary.component';

export const SharedComponent = [
  COMMON_COMPONENTS,
  COMMON_DIRECTIVES,
  COMMON_PIPES,
  OrderSummaryComponent
];
