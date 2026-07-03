import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Landing } from './pages/landing/landing';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Patientdashboard } from './pages/patient/patientdashboard/patientdashboard';
import { DoctorDashboard } from './pages/doctor/doctor-dashboard/doctor-dashboard';
import { ChangePassword } from './pages/doctor/change-password/change-password';
import { FindDoctor } from './pages/doctor/find-doctor/find-doctor';
import { BookAppointment } from './pages/patient/book-appointment/book-appointment';
import { DoctorAppointments } from './pages/doctor/doctor-appointments/doctor-appointments';
import { CreateHealthRecord } from './pages/doctor/create-health-record/create-health-record';
import { MyPatients } from './pages/doctor/my-patients/my-patients';
import { PatientDetails } from './pages/doctor/patient-details/patient-details';
import { TodayAppointments } from './pages/doctor/today-appointments/today-appointments';


const routes: Routes = [
  { path: '', component: Landing },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'patientdashboard', component: Patientdashboard },
  { path: 'doctor-dashboard', component: DoctorDashboard },
  { path: 'change-password', component: ChangePassword },
  { path: 'find-doctor', component: FindDoctor },
  { path: 'book-appointment', component: BookAppointment },
  { path: 'doctor-appointments', component: DoctorAppointments },
  { path: 'create-health-record', component: CreateHealthRecord },
  { path: 'my-patients', component: MyPatients },
  { path: 'patient-details/:id', component: PatientDetails },
  {
    path: 'today-appointments',
    component: TodayAppointments
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
