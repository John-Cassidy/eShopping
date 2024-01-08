import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CoreComponent } from './core';
import { HttpClient } from '@angular/common/http';
import { IPagination } from './shared/models/pagination';
import { IProduct } from './shared/models/products';
import { RouterOutlet } from '@angular/router';
import { SharedCompnent } from './shared';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, CoreComponent, SharedCompnent],
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
