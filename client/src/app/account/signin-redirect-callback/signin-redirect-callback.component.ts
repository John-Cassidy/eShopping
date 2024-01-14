import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { AcntService } from '../acnt.service';
import { CommonModule } from '@angular/common';
import { SharedComponent } from '../../shared';

@Component({
  selector: 'app-signin-redirect-callback',
  standalone: true,
  imports: [CommonModule, SharedComponent],
  template: `<div></div>`,
  styles: ``,
})
export class SigninRedirectCallbackComponent implements OnInit {
  returnUrl!: string;
  constructor(
    private _router: Router,
    private acntService: AcntService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.acntService.finishLogin().then((_) => {
      console.log('inside finish login');

      this._router.navigate(['/checkout'], { replaceUrl: true });
    });
  }
}
