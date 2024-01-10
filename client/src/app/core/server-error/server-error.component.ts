import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [SharedComponent, RouterModule],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.scss',
})
export class ServerErrorComponent {}
