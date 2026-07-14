import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
import { PortalLayout } from './layout/portal-layout/portal-layout';
import { ChangePassword } from './pages/change-password/change-password';
import { DoctorAppointments } from './pages/doctor/doctor-appointments/doctor-appointments';
import { DoctorDashboard } from './pages/doctor/doctor-dashboard/doctor-dashboard';
import { DoctorLeavePage } from './pages/doctor/doctor-leave/doctor-leave';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { BookAppointment } from './pages/patient/book-appointment/book-appointment';
import { PatientAppointments } from './pages/patient/patient-appointments/patient-appointments';
import { PatientDashboard } from './pages/patient/patient-dashboard/patient-dashboard';
import { PatientHealthRecords } from './pages/patient/patient-health-records/patient-health-records';
import { Register } from './pages/register/register';

export const routes: Routes = [
  {
    path: '',
    component: Home
  },
  {
    path: 'login',
    component: Login
  },
  {
    path: 'register',
    component: Register
  },
  {
    path: 'change-password',
    component: ChangePassword
  },
  {
    path: 'patient',
    component: PortalLayout,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Patient'] },
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        component: PatientDashboard
      },
      {
        path: 'appointments',
        component: PatientAppointments
      },
      {
        path: 'appointments/book',
        component: BookAppointment
      },
      {
        path: 'health-records',
        component: PatientHealthRecords
      }
    ]
  },
  {
    path: 'doctor',
    component: PortalLayout,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Doctor'] },
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        component: DoctorDashboard
      },
      {
        path: 'appointments',
        component: DoctorAppointments
      },
      {
        path: 'leave',
        component: DoctorLeavePage
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];
