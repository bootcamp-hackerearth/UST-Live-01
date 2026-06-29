import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/landing/landing').then(m => m.Landing)
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./pages/login/login').then(m => m.Login)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./pages/register/register').then(m => m.Register)
  },
  {
    path: 'patient/dashboard',
    canActivate: [authGuard, roleGuard],
    data: {
      roles: ['Patient']
    },
    loadComponent: () =>
      import('./pages/patient-dashboard/patient-dashboard').then(m => m.PatientDashboard)
  },
  {
    path: 'patient/doctors',
    canActivate: [authGuard, roleGuard],
    data: {
      roles: ['Patient']
    },
    loadComponent: () =>
      import('./pages/patient-doctors/patient-doctors').then(m => m.PatientDoctors)
  },
  {
  path: 'patient/book/:doctorId',
  canActivate: [authGuard, roleGuard],
  data: {
    roles: ['Patient']
  },
  loadComponent: () =>
    import('./pages/book-appointment/book-appointment').then(m => m.BookAppointment)
},
{
  path: 'patient/appointments',
  canActivate: [authGuard, roleGuard],
  data: {
    roles: ['Patient']
  },
  loadComponent: () =>
    import('./pages/patient-appointments/patient-appointments').then(m => m.PatientAppointments)
},
{
  path: 'patient/history',
  canActivate: [authGuard, roleGuard],
  data: {
    roles: ['Patient']
  },
  loadComponent: () =>
    import('./pages/patient-history/patient-history').then(m => m.PatientHistory)
},
  {
    path: 'doctor/dashboard',
    canActivate: [authGuard, roleGuard],
    data: {
      roles: ['Doctor']
    },
    loadComponent: () =>
      import('./pages/doctor-dashboard/doctor-dashboard').then(m => m.DoctorDashboard)
  },
  {
    path: '**',
    redirectTo: ''
  }
];

