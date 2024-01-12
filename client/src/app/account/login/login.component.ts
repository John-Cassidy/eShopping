import { AcntService } from '../acnt.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  title = 'Login';

  constructor(private acntService: AcntService) {}

  login() {
    this.acntService.login();
  }
}
