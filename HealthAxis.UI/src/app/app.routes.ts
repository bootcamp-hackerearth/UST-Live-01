import { Routes } from '@angular/router';

import { roleGuard } from './core/guards/role.guard';

import { DashboardLayout } from './layouts/dashboard-layout/dashboard-layout';
import { PublicLayout } from './layouts/public-layout/public-layout';

import { Landing } from './pages/landing/landing';
import { Login } from './pages/login/login';
import { PatientRegister } from './pages/patient-register/patient-register';

import { BookAppointment } from './patient/book-appointment/book-appointment';
import { ContactUs } from './patient/contact-us/contact-us';
import { HealthRecords } from './patient/health-records/health-records';
import { MyAppointments } from './patient/my-appointments/my-appointments';
import { PatientDashboard } from './patient/patient-dashboard/patient-dashboard';
import { PatientProfile } from './patient/patient-profile/patient-profile';

import { CompletedAppointments } from './doctor/completed-appointments/completed-appointments';
import { DoctorDashboard } from './doctor/doctor-dashboard/doctor-dashboard';
import { DoctorHealthRecords } from './doctor/doctor-health-records/doctor-health-records';
import { DoctorProfile } from './doctor/doctor-profile/doctor-profile';
import { UpcomingAppointments } from './doctor/upcoming-appointments/upcoming-appointments';

export const routes: Routes = [
  {
    path: '',
    component: PublicLayout,
    children: [
      {
        path: '',
        component: Landing
      },
      {
        path: 'login',
        component: Login
      },
      {
        path: 'register',
        component: PatientRegister
      }
    ]
  },
  {
    path: 'patient',
    component: DashboardLayout,
    canActivate: [
      roleGuard
    ],
    data: {
      roles: ['Patient']
    },
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
        path: 'my-appointments',
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
        path: 'change-password',
        redirectTo: 'profile',
        pathMatch: 'full'
      },
      {
        path: 'contact-us',
        component: ContactUs
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
    component: DashboardLayout,
    canActivate: [
      roleGuard
    ],
    data: {
      roles: ['Doctor']
    },
    children: [
      {
        path: 'dashboard',
        component: DoctorDashboard
      },
      {
        path: 'upcoming-appointments',
        component: UpcomingAppointments
      },
      {
        path: 'completed-appointments',
        component: CompletedAppointments
      },
      {
        path: 'health-records',
        component: DoctorHealthRecords
      },
      {
        path: 'profile',
        component: DoctorProfile
      },
      {
        path: 'change-password',
        redirectTo: 'profile',
        pathMatch: 'full'
      },
      {
        path: 'contact-us',
        component: ContactUs
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