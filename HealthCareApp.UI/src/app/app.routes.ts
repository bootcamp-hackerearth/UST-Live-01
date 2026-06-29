import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { PatientDashboard } from './pages/patient/patient-dashboard/patient-dashboard';

export const routes: Routes = [
  {
    path: '',
    component: Home
  },
  {
    path: 'home',
    component: Home
  },
  {
    path: 'patient/dashboard',
    component: PatientDashboard
  },
  {
    path: '**',
    redirectTo: ''
  }
];