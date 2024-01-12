import { Component, OnInit } from '@angular/core';

import { AcntService } from '../acnt.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signout-redirect-callback.component',
  standalone: true,
  imports: [],
  template: `<div></div>`,
  styles: ``,
})
export class SignoutRedirectCallbackComponentComponent implements OnInit {
  constructor(private _router: Router, private acntService: AcntService) {}

  ngOnInit(): void {
    this.acntService.finishLogout().then((_) => {
      this._router.navigate(['/'], { replaceUrl: true });
    });
  }
}
