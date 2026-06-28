import { Routes } from '@angular/router';

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
    path: 'patient/doctors',
    loadComponent: () =>
      import('./pages/patient-doctors/patient-doctors').then(m => m.PatientDoctors)
  },
  {
    path: 'doctor/dashboard',
    loadComponent: () =>
      import('./pages/doctor-dashboard/doctor-dashboard').then(m => m.DoctorDashboard)
  },
  {
    path: '**',
    redirectTo: ''
  }
];

