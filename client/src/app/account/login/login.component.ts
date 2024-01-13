import { AcntService } from '../acnt.service';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, SharedComponent],
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
