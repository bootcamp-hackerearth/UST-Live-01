import { Routes } from '@angular/router';

import { Home } from './features/landing/home/home';
import { Register } from './features/auth/register/register';
import { Login } from './features/auth/login/login';

import { PatientDashboard } from './features/patient/patient-dashboard/patient-dashboard';
import { DoctorSearch } from './features/patient/doctor-search/doctor-search';
import { MyAppointments } from './features/patient/my-appointments/my-appointments';
import { HealthHistory } from './features/patient/health-history/health-history';

import { DoctorDashboard } from './features/doctor/doctor-dashboard/doctor-dashboard';

export const routes: Routes = [
  {
    path: '',
    component: Home,
    pathMatch: 'full'
  },
  {
    path: 'register',
    component: Register
  },
  {
    path: 'login',
    component: Login
  },

  {
    path: 'patient/dashboard',
    component: PatientDashboard
  },
  {
    path: 'patient/doctors',
    component: DoctorSearch
  },
  {
    path: 'patient/appointments',
    component: MyAppointments
  },
  {
    path: 'patient/health-history',
    component: HealthHistory
  },

  {
    path: 'doctor/dashboard',
    component: DoctorDashboard
  },

  {
    path: '**',
    redirectTo: ''
  }
];
