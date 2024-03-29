import {
  Basket,
  BasketCheckout,
  IBasket,
  IBasketCheckout,
  IBasketItem,
  IBasketTotal,
} from '../shared/models/basket';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { AcntService } from '../account/acnt.service';
import { BehaviorSubject } from 'rxjs';
import { IProduct } from '../shared/models/products';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  // #region properties
  // private
  private basketSource = new BehaviorSubject<IBasket | null>(null);
  private basketTotal = new BehaviorSubject<IBasketTotal | null>(null);
  // public
  baseUrl = 'http://localhost:9010';
  basketSource$ = this.basketSource.asObservable();
  basketTotal$ = this.basketTotal.asObservable();
  // #endregion

  constructor(
    private http: HttpClient,
    private acntService: AcntService,
    private router: Router
  ) {}

  // #region public methods
  getBasket(userName: string) {
    return this.http
      .get<IBasket>(`${this.baseUrl}/Basket/GetBasket/frizzo`) // TODO: replace frizzo with ${userName} when Authentication Provider configured
      .subscribe({
        next: (basket) => {
          this.basketSource.next(basket);
          this.calculateBasketTotal();
        },
        error: (err) => console.log(err),
      });
  }

  setBasket(basket: IBasket) {
    return this.http
      .post<IBasket>(`${this.baseUrl}/Basket/CreateBasket`, basket)
      .subscribe({
        next: (basket) => {
          this.basketSource.next(basket);
          this.calculateBasketTotal();
        },
        error: (err) => console.log(err),
      });
  }

  checkoutBasket(basket: IBasket) {
    // create create basketCheckout = new BasketCheckout assigning values from basket
    const basketCheckout: IBasketCheckout = {
      userName: basket.userName,
      totalPrice: basket.totalPrice,
      firstName: 'Frank',
      lastName: 'Rizzo',
      emailAddress: 'frankrizzo@gmail.com',
      addressLine: '50 Main St.',
      country: 'USA',
      state: 'MA',
      zipCode: '02138',
      cardName: 'Visa',
      cardNumber: '1234567890123456',
      expiration: '12/25',
      cvv: '123',
      paymentMethod: 1,
    };

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: this.acntService.authorizationHeaderValue,
      }),
    };
    return this.http
      .post<IBasket>(
        this.baseUrl + '/Basket/Checkout',
        basketCheckout,
        httpOptions
      )
      .subscribe({
        next: (basket) => {
          this.basketSource.next(null);
          this.router.navigateByUrl('/');
        },
        error: (err) => {
          console.log('Error Occurred calling basket checkout');
          console.log(err);
        },
      });
  }

  getCurrentBasket() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasket() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if (!basket) return;
    const foundItemIndex = basket.items.findIndex(
      (x) => x.productId == item.productId
    );
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if (!basket) return;
    if (basket.items.some((x) => x.productId == item.productId)) {
      basket.items = basket.items.filter((i) => i.productId !== item.productId);
      if (basket.items.length > 0) {
        this.setBasket(basket);
      } else {
        this.deleteBasket(basket.userName);
      }
    }
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if (!basket) return;
    const foundItemIndex = basket.items.findIndex(
      (x) => x.productId == item.productId
    );
    if (basket.items[foundItemIndex].quantity > 1) {
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }
  }

  // #endregion

  // #region private methods
  private mapProductItemToBasketItem(item: IProduct): IBasketItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      imageFile: item.imageFile,
      quantity: 0,
    };
  }
  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_username', 'frizzo'); // TODO: replace frizzo with LoggedIn User
    return basket;
  }
  private addOrUpdateItem(
    items: IBasketItem[],
    itemToAdd: IBasketItem,
    quantity: number
  ): IBasketItem[] {
    //if we have the item in basket which matches the Id, then we can get here
    const item = items.find((x) => x.productId == itemToAdd.productId);
    if (item) {
      item.quantity += quantity;
    } else {
      itemToAdd.quantity = quantity;
      //then add the items in the basket
      items.push(itemToAdd);
    }
    return items;
  }
  private deleteBasket(userName: string) {
    return this.http
      .delete(`${this.baseUrl}/Basket/DeleteBasket/${userName}`)
      .subscribe({
        next: () => {
          this.basketSource.next(null);
          localStorage.removeItem('basket_username');
        },
        error: (err) => {
          console.log('Error Occurred while deleting basket');
          console.log(err);
        },
      });
  }
  private calculateBasketTotal() {
    const basket = this.getCurrentBasket();
    if (!basket) return;
    //We are going to loop over in array and calculate total
    const shipping = 0;
    const subtotal = basket.items.reduce((a, b) => b.price * b.quantity + a, 0);
    const total = shipping + subtotal;
    this.basketTotal.next({ total });
  }
  // #endregion
}
