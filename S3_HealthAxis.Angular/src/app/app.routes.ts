import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
import { doctorPasswordGuard } from './core/guards/doctor-password.guard';

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

  // ==========================
  // PATIENT LAYOUT
  // ==========================
  {
    path: 'patient',
    canActivate: [authGuard, roleGuard],
    data: {
      roles: ['Patient']
    },
    loadComponent: () =>
      import('./layouts/patient-layout/patient-layout').then(
        m => m.PatientLayout
      ),
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./pages/patient-dashboard/patient-dashboard').then(
            m => m.PatientDashboard
          )
      },
      {
        path: 'doctors',
        loadComponent: () =>
          import('./pages/patient-doctors/patient-doctors').then(
            m => m.PatientDoctors
          )
      },
      {
        path: 'book/:doctorId',
        loadComponent: () =>
          import('./pages/book-appointment/book-appointment').then(
            m => m.BookAppointment
          )
      },
      {
        path: 'appointments',
        loadComponent: () =>
          import('./pages/patient-appointments/patient-appointments').then(
            m => m.PatientAppointments
          )
      },
      {
        path: 'history',
        loadComponent: () =>
          import('./pages/patient-history/patient-history').then(
            m => m.PatientHistory
          )
      },
      {
        path: 'profile',
        loadComponent: () =>
          import('./pages/patient-profile/patient-profile').then(
            m => m.PatientProfile
          )
      }
    ]
  },

  // ==========================
  // DOCTOR PASSWORD CHANGE
  // ==========================
  {
    path: 'doctor/change-password-required',
    canActivate: [authGuard, roleGuard],
    data: {
      roles: ['Doctor']
    },
    loadComponent: () =>
      import(
        './pages/doctor-change-password-required/doctor-change-password-required'
      ).then(m => m.DoctorChangePasswordRequired)
  },

  // ==========================
  // DOCTOR LAYOUT
  // ==========================
  {
    path: 'doctor',
    canActivate: [authGuard, roleGuard, doctorPasswordGuard],
    data: {
      roles: ['Doctor']
    },
    loadComponent: () =>
      import('./layouts/doctor-layout/doctor-layout').then(
        m => m.DoctorLayout
      ),
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./pages/doctor-dashboard/doctor-dashboard').then(
            m => m.DoctorDashboard
          )
      },
      {
        path: 'schedule',
        loadComponent: () =>
          import('./pages/doctor-schedule/doctor-schedule').then(
            m => m.DoctorSchedule
          )
      },
      {
        path: 'patients',
        loadComponent: () =>
          import('./pages/doctor-patients/doctor-patients').then(
            m => m.DoctorPatients
          )
      }
    ]
  },

  // ==========================
  // FALLBACK
  // ==========================
  {
    path: '**',
    redirectTo: ''
  }
];

