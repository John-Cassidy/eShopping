import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-unauthenticated',
  standalone: true,
  imports: [SharedComponent, RouterModule],
  templateUrl: './unauthenticated.component.html',
  styleUrl: './unauthenticated.component.scss',
})
export class UnauthenticatedComponent {}
