import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { SharedComponent } from '../shared';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [CommonModule, SharedComponent, LoginComponent, RegisterComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.scss',
})
export class AccountComponent {}
