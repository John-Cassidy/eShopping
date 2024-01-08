import { HttpClient } from '@angular/common/http';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/products';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  constructor(private httpClient: HttpClient) {}

  baseUrl = 'http://localhost:9010/';

  getProducts() {
    return this.httpClient.get<IPagination<IProduct[]>>(
      this.baseUrl + 'Catalog/GetAllProducts'
    );
  }
}
