import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [SharedComponent, RouterModule],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss',
})
export class NotFoundComponent {}
