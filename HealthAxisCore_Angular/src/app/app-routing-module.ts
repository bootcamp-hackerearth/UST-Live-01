import { RouterModule, Routes } from '@angular/router';

import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';

import { PatientLayout } from './pages/patient/patient-layout/patient-layout'
import { PatientDashboard } from './pages/patient/patient-dashboard/patient-dashboard';
import { PatientProfile } from './pages/patient/patient-profile/patient-profile';
import { FindDoctors } from './pages/patient/find-doctors/find-doctors';
import { MyAppointments } from './pages/patient/my-appointments/my-appointments';
import { HealthHistory } from './pages/patient/health-history/health-history';
import { NgModule } from '@angular/core';
import { BookAppointment } from './pages/patient/book-appointment/book-appointment';
import { DoctorLayout } from './doctor/doctor-layout/doctor-layout';
import { DoctorDashboard } from './doctor/doctor-dashboard/doctor-dashboard';
import { TodaySchedule } from './doctor/today-schedule/today-schedule';
import { DoctorPatients } from './doctor/doctor-patients/doctor-patients';
import { DoctorPatientProfile } from './doctor/doctor-patient-profile/doctor-patient-profile';
import { AddHealthRecord } from './doctor/add-health-record/add-health-record';

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
    path: 'patient',
    component: PatientLayout,
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
    children: [
      {
        path: 'dashboard',
        component: DoctorDashboard
      },
      {
        path: 'schedule',
        component: TodaySchedule
      },
      {
        path: 'patients',
        component: DoctorPatients
      },
      {
        path: 'patient/:patientId',
        component: DoctorPatientProfile
      },
      {
        path: 'add-health-record/:appointmentId',
        component: AddHealthRecord
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
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
