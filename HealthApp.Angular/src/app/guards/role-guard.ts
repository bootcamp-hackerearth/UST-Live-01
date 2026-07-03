import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {

    const userRole = localStorage.getItem('role');

    const expectedRole = route.data['role'];

    if (userRole === expectedRole) {
      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }
}
