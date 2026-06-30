import { Routes } from '@angular/router';

import { Home } from './features/landing/home/home';
import { Register } from './features/auth/register/register';
import { Login } from './features/auth/login/login';

import { PatientLayout } from './layouts/patient-layout/patient-layout';
import { PatientDashboard } from './features/patient/patient-dashboard/patient-dashboard';
import { DoctorSearch } from './features/patient/doctor-search/doctor-search';
import { BookAppointment } from './features/patient/book-appointment/book-appointment';
import { MyAppointments } from './features/patient/my-appointments/my-appointments';
import { HealthHistory } from './features/patient/health-history/health-history';
import { MyInformation } from './features/patient/my-information/my-information';
import { ChangePassword } from './features/patient/change-password/change-password';

import { DoctorLayout } from './layouts/doctor-layout/doctor-layout';
import { DoctorDashboard } from './features/doctor/doctor-dashboard/doctor-dashboard';
import { DoctorSchedule } from './features/doctor/doctor-schedule/doctor-schedule';
import { PatientProfile } from './features/doctor/patient-profile/patient-profile';
import { DoctorMyInformation } from './features/doctor/doctor-my-information/doctor-my-information';
import { DoctorChangePassword } from './features/doctor/doctor-change-password/doctor-change-password';
import { AddHealthRecord } from './features/doctor/add-health-record/add-health-record';

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
    path: 'patient',
    component: PatientLayout,
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
        path: 'doctors',
        component: DoctorSearch
      },
      {
        path: 'book-appointment/:doctorId',
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
        path: 'my-information',
        component: MyInformation
      },
      {
        path: 'change-password',
        component: ChangePassword
      }
    ]
  },

 {
  path: 'doctor',
  component: DoctorLayout,
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
      path: 'schedule',
      component: DoctorSchedule
    },
    {
      path: 'patient-profile',
      component: PatientProfile
    },
    {
      path: 'patient-profile/:patientId',
      component: PatientProfile
    },
    {
      path: 'add-health-record',
      component: AddHealthRecord
    },
    {
      path: 'add-health-record/:appointmentId',
      component: AddHealthRecord
    },
    {
      path: 'my-information',
      component: DoctorMyInformation
    },
    {
      path: 'change-password',
      component: DoctorChangePassword
    }
  ]
},

  {
    path: '**',
    redirectTo: ''
  }
];