import { BreadcrumbModule, BreadcrumbService } from 'xng-breadcrumb';

import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, SharedComponent, BreadcrumbModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  constructor(public bcService: BreadcrumbService) {}
}
