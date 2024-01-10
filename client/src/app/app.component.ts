import { RouterModule, RouterOutlet } from '@angular/router';

import { Component } from '@angular/core';
import { CoreComponent } from './core';
import { HomeComponent } from './home/home.component';
import { HttpClient } from '@angular/common/http';
import { IPagination } from './shared/models/pagination';
import { IProduct } from './shared/models/products';
import { SharedComponent } from './shared';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CoreComponent,
    SharedComponent,
    HomeComponent,
    RouterModule,
  ],
  providers: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'eShopping';
  products: IProduct[] = [];

  constructor(private http: HttpClient) {
    // constructor logic here
  }

  ngOnInit() {
    this.http
      .get<IPagination<IProduct[]>>(
        'http://localhost:9010/Catalog/GetAllProducts'
      )
      .subscribe({
        next: (response) => {
          this.products = response.data as IProduct[];
          console.log(this.products);
        },
        error: (err) => console.log(err),
        complete: () => console.log('Catalog API call compoleted'),
      });
  }
}
