import { Component } from '@angular/core';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-unauthenticated',
  standalone: true,
  imports: [SharedComponent],
  templateUrl: './unauthenticated.component.html',
  styleUrl: './unauthenticated.component.scss',
})
export class UnauthenticatedComponent {}
