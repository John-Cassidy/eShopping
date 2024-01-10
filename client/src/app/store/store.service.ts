import { HttpClient, HttpParams } from '@angular/common/http';

import { IBrand } from '../shared/models/brand';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/products';
import { IType } from '../shared/models/type';
import { Injectable } from '@angular/core';
import { StoreParams } from '../shared/models/storeParams';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  constructor(private httpClient: HttpClient) {}

  baseUrl = 'http://localhost:9010/';

  getProductById(id: string) {
    return this.httpClient.get<IProduct>(
      this.baseUrl + 'Catalog/GetProductById/' + id
    );
  }

  getProducts(storeParams: StoreParams) {
    let params = new HttpParams();
    if (storeParams.brandId) {
      params = params.append('brandId', storeParams.brandId);
    }
    if (storeParams.typeId) {
      params = params.append('typeId', storeParams.typeId);
    }
    if (storeParams.search) {
      params = params.append('search', storeParams.search);
    }

    params = params.append('sort', storeParams.sort);
    params = params.append('pageIndex', storeParams.pageNumber);
    params = params.append('pageSize', storeParams.pageSize);

    return this.httpClient.get<IPagination<IProduct[]>>(
      this.baseUrl + 'Catalog/GetAllProducts',
      { params }
    );
  }

  getBrands() {
    return this.httpClient.get<IBrand[]>(this.baseUrl + 'Catalog/GetAllBrands');
  }

  getTypes() {
    return this.httpClient.get<IType[]>(this.baseUrl + 'Catalog/GetAllTypes');
  }
}
