import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from '@angular/router';

import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {

    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login'], {
        queryParams: {
          returnUrl: state.url
        }
      });

      return false;
    }

    const expectedRoles = route.data['roles'] as string[];
    const userRole = (this.auth.getRole() || '').trim();

    if (expectedRoles && expectedRoles.includes(userRole)) {
      return true;
    }

    // Redirect based on actual logged-in role
    if (userRole === 'Patient') {
      this.router.navigate(['/patient']);
    }

    else if (userRole === 'Doctor') {
      this.router.navigate(['/doctor']);
    }

    else if (userRole === 'Admin') {
      window.location.replace(this.auth.getAdminPortalBridgeUrl());
    }

    else {
      this.auth.logout();

      this.router.navigate(['/login']);
    }

    return false;
  }
}