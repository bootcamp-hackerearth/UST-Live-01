import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { doctorGuard } from './core/guards/doctor-guard';
import { patientGuard } from './core/guards/patient-guard';

import { Landing } from './pages/landing/landing';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';

import { Patientdashboard } from './pages/patient/patientdashboard/patientdashboard';
import { BookAppointment } from './pages/patient/book-appointment/book-appointment';
import { MyAppointments } from './pages/patient/my-appointments/my-appointments';
import { MyHealthRecords } from './pages/patient/my-health-records/my-health-records';
import { FindDoctor } from './pages/doctor/find-doctor/find-doctor';

import { DoctorDashboard } from './pages/doctor/doctor-dashboard/doctor-dashboard';
import { ChangePassword } from './pages/doctor/change-password/change-password';
import { DoctorAppointments } from './pages/doctor/doctor-appointments/doctor-appointments';
import { CreateHealthRecord } from './pages/doctor/create-health-record/create-health-record';
import { MyPatients } from './pages/doctor/my-patients/my-patients';
import { PatientDetails } from './pages/doctor/patient-details/patient-details';
import { TodayAppointments } from './pages/doctor/today-appointments/today-appointments';

const routes: Routes = [

  // Public Routes
  { path: '', component: Landing },
  { path: 'login', component: Login },
  { path: 'register', component: Register },

  // Patient Routes
  {
    path: 'patient',
    canActivate: [patientGuard],
    children: [
      { path: 'dashboard', component: Patientdashboard },
      { path: 'find-doctor', component: FindDoctor },
      { path: 'book-appointment', component: BookAppointment },
      { path: 'appointments', component: MyAppointments },
      { path: 'health-records', component: MyHealthRecords }
    ]
  },

  // Doctor Routes
  {
    path: 'doctor',
    canActivate: [doctorGuard],
    children: [
      { path: 'dashboard', component: DoctorDashboard },
      { path: 'change-password', component: ChangePassword },
      { path: 'appointments', component: DoctorAppointments },
      { path: 'today-appointments', component: TodayAppointments },
      { path: 'patients', component: MyPatients },
      { path: 'patient-details/:id', component: PatientDetails },
      { path: 'create-health-record', component: CreateHealthRecord }
    ]
  },

  // Fallback
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
