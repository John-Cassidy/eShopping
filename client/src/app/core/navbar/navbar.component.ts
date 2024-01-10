import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [SharedComponent, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {}
