import { AcntService } from '../../account/acnt.service';
import { BasketService } from '../../basket/basket.service';
import { Component } from '@angular/core';
import { IBasketItem } from '../../shared/models/basket';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [SharedComponent],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  constructor(
    public basketService: BasketService,
    private acntService: AcntService
  ) {}
  ngOnInit(): void {
    console.log(`current user:`);
    this.acntService.currentUser$.subscribe({
      next: (res) => {
        this.isUserAuthenticated = res;
        console.log(this.isUserAuthenticated);
      },
      error: (err) => {
        console.log(
          `An error occurred while setting isUserAuthenticated flag.`
        );
      },
    });
  }
  public isUserAuthenticated: boolean = false;

  getBasketCount(items: IBasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  public login = () => {
    this.acntService.login();
  };
  public logout = () => {
    this.acntService.signout();
  };
}
