import { RouterModule, RouterOutlet } from '@angular/router';

import { Component } from '@angular/core';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterOutlet, SharedComponent, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {}
