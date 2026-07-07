import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/home/home')
        .then(m => m.Home)
  },
  {
    path: 'patient/dashboard',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Patient']
    },
    loadComponent: () =>
      import('./pages/patient/patient-dashboard/patient-dashboard')
        .then(m => m.PatientDashboard)
  },
  {
    path: 'doctor/dashboard',
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Doctor']
    },
    loadComponent: () =>
      import('./pages/doctor/doctor-dashboard/doctor-dashboard')
        .then(m => m.DoctorDashboard)
  },
  {
    path: 'access-denied',
    loadComponent: () =>
      import('./pages/errros/access-denied/access-denied')
        .then(m => m.AccessDenied)
  },
  {
    path: 'route-unavailable',
    loadComponent: () =>
      import('./pages/errros/route-unavailable/route-unavailable')
        .then(m => m.RouteUnavailable)
  },
  {
    path: '**',
    redirectTo: 'route-unavailable'
  }
];