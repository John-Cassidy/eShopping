import { Component, OnInit } from '@angular/core';

import { AcntService } from '../acnt.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-signout-redirect-callback.component',
  standalone: true,
  imports: [CommonModule, SharedComponent],
  template: `<div></div>`,
  styles: ``,
})
export class SignoutRedirectCallbackComponent implements OnInit {
  constructor(private _router: Router, private acntService: AcntService) {}

  ngOnInit(): void {
    this.acntService.finishLogout().then((_) => {
      this._router.navigate(['/'], { replaceUrl: true });
    });
  }
}
