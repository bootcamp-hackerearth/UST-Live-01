import { Routes } from '@angular/router';

import { Home } from './pages/home/home';
import { Login } from './pages/auth/login/login';
import { PatientRegister } from './pages/auth/patient-register/patient-register';

import { PatientLayout } from './pages/patient/patient-layout/patient-layout';
import { PatientDashboard } from './pages/patient/patient-dashboard/patient-dashboard';
import { BookAppointment } from './pages/patient/book-appointment/book-appointment';
import { MyAppointments } from './pages/patient/appointments/appointments';
import { HealthRecords } from './pages/patient/health-records/health-records';
import { PatientProfile } from './pages/patient/profile/profile';

import { DoctorLayout } from './pages/doctor/doctor-layout/doctor-layout';
import { DoctorDashboard } from './pages/doctor/doctor-dashboard/doctor-dashboard';
import { DoctorAppointments } from './pages/doctor/doctor-appointments/doctor-appointments';
import { DoctorHealthRecords } from './pages/doctor/doctor-health-records/doctor-health-records';
import { DoctorProfile } from './pages/doctor/profile/profile';
import { DoctorLeave } from './pages/doctor/leave/leave';

import { authGuard } from './core/guards/auth.guard';
import { doctorGuard, patientGuard } from './core/guards/role.guard';


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
    component: PatientRegister
  },
  {
    path: 'patient',
    component: PatientLayout,
    canActivate: [authGuard, patientGuard],
    children: [
      {
        path: 'dashboard',
        component: PatientDashboard
      },
      {
        path: 'book-appointment',
        component: BookAppointment
      },
      {
        path: 'appointments',
        component: MyAppointments
      },
      {
        path: 'health-records',
        component: HealthRecords
      },
      {
        path: 'profile',
        component: PatientProfile
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: 'doctor',
    component: DoctorLayout,
    canActivate: [authGuard, doctorGuard],
    children: [
      {
        path: 'dashboard',
        component: DoctorDashboard
      },
      {
        path: 'appointments',
        component: DoctorAppointments
      },
      {
        path: 'health-records',
        component: DoctorHealthRecords
      },
      {
        path: 'health-records/:patientId',
        component: DoctorHealthRecords
      },
      {
        path: 'profile',
        component: DoctorProfile
      },
      {
        path: 'leave',
          component: DoctorLeave
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];