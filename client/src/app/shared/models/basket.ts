export interface IBasket {
  userName: string;
  items: IBasketItem[];
  totalPrice: number;
}

export interface IBasketItem {
  quantity: number;
  imageFile: string;
  price: number;
  productId: string;
  productName: string;
}

export class Basket implements IBasket {
  userName: string = 'frizzo';
  totalPrice: number = 0;
  items: IBasketItem[] = [];
}

export interface IBasketCheckout {
  userName: string;
  totalPrice: number;
  firstName: string;
  lastName: string;
  emailAddress: string;
  addressLine: string;
  country: string;
  state: string;
  zipCode: string;
  cardName: string;
  cardNumber: string;
  expiration: string;
  cvv: string;
  paymentMethod: number;
}

export class BasketCheckout implements IBasketCheckout {
  userName: string = '';
  totalPrice: number = 0;
  firstName: string = '';
  lastName: string = '';
  emailAddress: string = '';
  addressLine: string = '';
  country: string = '';
  state: string = '';
  zipCode: string = '';
  cardName: string = '';
  cardNumber: string = '';
  expiration: string = '';
  cvv: string = '';
  paymentMethod: number = 0;
}

export interface IBasketTotal {
  total: number;
}
