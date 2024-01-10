import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SharedComponent } from '../shared';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, SharedComponent, CarouselModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {}
