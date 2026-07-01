import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Forbidden } from './pages/forbidden/forbidden';

import { PatientLayout } from './pages/patient/patient-layout/patient-layout';
import { PatientDashboard } from './pages/patient/patient-dashboard/patient-dashboard';
import { PatientProfile } from './pages/patient/patient-profile/patient-profile';
import { FindDoctors } from './pages/patient/find-doctors/find-doctors';
import { BookAppointment } from './pages/patient/book-appointment/book-appointment';
import { MyAppointments } from './pages/patient/my-appointments/my-appointments';
import { HealthHistory } from './pages/patient/health-history/health-history';

import { DoctorLayout } from './pages/doctor/doctor-layout/doctor-layout';
import { DoctorDashboard } from './pages/doctor/doctor-dashboard/doctor-dashboard';
import { TodaySchedule } from './pages/doctor/today-schedule/today-schedule';
import { DoctorPatients } from './pages/doctor/doctor-patients/doctor-patients';
import { DoctorPatientProfile } from './pages/doctor/doctor-patient-profile/doctor-patient-profile';
import { AddHealthRecord } from './pages/doctor/add-health-record/add-health-record';
import { ChangePassword } from './pages/doctor/change-password/change-password';
import { firstLoginGuard } from './core/guards/first-login.guard';
import { DoctorProfile } from './pages/doctor/doctor-profile/doctor-profile';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

const routes: Routes = [
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
    path: 'forbidden',
    component: Forbidden
  },
  {
    path: 'patient',
    component: PatientLayout,
    canActivate: [
      authGuard,
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
        path: 'profile',
        component: PatientProfile
      },
      {
        path: 'doctors',
        component: FindDoctors
      },
      {
        path: 'book/:doctorId',
        component: BookAppointment
      },
      {
        path: 'appointments',
        component: MyAppointments
      },
      {
        path: 'health-history',
        component: HealthHistory
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
    canActivate: [
      authGuard,
      roleGuard
    ],
    data: {
      roles: ['Doctor']
    },
    children: [
      {
        path: 'change-password',
        component: ChangePassword
      },
      {
        path: 'profile',
        component: DoctorProfile,
        canActivate: [
          firstLoginGuard
        ]
      },
      {
        path: 'dashboard',
        component: DoctorDashboard,
        canActivate: [
          firstLoginGuard
        ]
      },
      {
        path: 'schedule',
        component: TodaySchedule,
        canActivate: [
          firstLoginGuard
        ]
      },
      {
        path: 'patients',
        component: DoctorPatients,
        canActivate: [
          firstLoginGuard
        ]
      },
      {
        path: 'patient/:patientId',
        component: DoctorPatientProfile,
        canActivate: [
          firstLoginGuard
        ]
      },
      {
        path: 'add-health-record/:appointmentId',
        component: AddHealthRecord,
        canActivate: [
          firstLoginGuard
        ]
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

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {
}
