import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { TokenService } from '../services/token.service';

export const doctorPasswordGuard: CanActivateFn = () => {
  const tokenService = inject(TokenService);
  const router = inject(Router);

  const role = tokenService.getRole()?.toLowerCase();

  if (role === 'doctor' && tokenService.getMustChangePassword()) {
    router.navigate(['/doctor/change-password-required']);
    return false;
  }

  return true;
};

    