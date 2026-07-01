import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { PortalLayout } from './layout/portal-layout/portal-layout';
import { PatientDashboard } from './pages/patient/patient-dashboard/patient-dashboard';
import { PatientAppointments } from './pages/patient/patient-appointments/patient-appointments';
import { BookAppointment } from './pages/patient/book-appointment/book-appointment';
import { PatientHealthRecords } from './pages/patient/patient-health-records/patient-health-records';
import { ChangePassword } from './pages/change-password/change-password';
import { DoctorDashboard } from './pages/doctor/doctor-dashboard/doctor-dashboard';
import { DoctorAppointments } from './pages/doctor/doctor-appointments/doctor-appointments';

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
    }
  ]
},
  {
    path: '**',
    redirectTo: ''
  }
];