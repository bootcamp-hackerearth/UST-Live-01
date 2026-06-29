import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/home/home')
        .then(m => m.Home)
  },
  {
    path: 'patient/dashboard',
    loadComponent: () =>
      import('./pages/patient/patient-dashboard/patient-dashboard')
        .then(m => m.PatientDashboard)
  },
  {
    path: 'doctor/dashboard',
    loadComponent: () =>
      import('./pages/doctor/doctor-dashboard/doctor-dashboard')
        .then(m => m.DoctorDashboard)
  },
  {
    path: '**',
    redirectTo: ''
  }
];