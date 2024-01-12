import { CanActivateFn, Router } from '@angular/router';

import { AcntService } from '../../account/acnt.service';
import { map } from 'rxjs';

export function authGuardFactory(
  router: Router,
  acntService: AcntService
): CanActivateFn {
  return (route, state) => {
    return acntService.currentUser$.pipe(
      map((auth) => {
        if (auth) return true;
        else {
          router.navigate(['/account/login'], {
            queryParams: { returnUrl: state.url },
            replaceUrl: true,
          });
          return false;
        }
      })
    );
  };
}
