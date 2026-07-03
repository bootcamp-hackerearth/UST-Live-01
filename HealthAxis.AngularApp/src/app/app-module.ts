import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Landing } from './pages/landing/landing';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Patientdashboard } from './pages/patient/patientdashboard/patientdashboard';
import { DoctorDashboard } from './pages/doctor/doctor-dashboard/doctor-dashboard';
import { HttpClientModule } from '@angular/common/http';
import { ChangePassword } from './pages/doctor/change-password/change-password';
import { FindDoctor } from './pages/doctor/find-doctor/find-doctor';
import { BookAppointment } from './pages/patient/book-appointment/book-appointment';
import { DoctorAppointments } from './pages/doctor/doctor-appointments/doctor-appointments';
import { CreateHealthRecord } from './pages/doctor/create-health-record/create-health-record';
import { MyPatients } from './pages/doctor/my-patients/my-patients';
import { PatientDetails } from './pages/doctor/patient-details/patient-details';
import { TodayAppointments } from './pages/doctor/today-appointments/today-appointments';

@NgModule({
  declarations: [
    App,
    Landing,
    Login,
    Register,
    Patientdashboard,
    DoctorDashboard,
    ChangePassword,
    FindDoctor,
    BookAppointment,
    DoctorAppointments,
    CreateHealthRecord,
    MyPatients,
    PatientDetails,
    TodayAppointments,
  ],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule],
  providers: [provideBrowserGlobalErrorListeners()],
  bootstrap: [App],
})
export class AppModule {}
