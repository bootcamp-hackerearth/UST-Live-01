import { Routes } from '@angular/router';

import { Home } from './home/home';
import { LoginComponent } from './login/login-request.model';
import { Register } from './register/register';
import { DLeave } from './Doctors/d_leave/d_leave';


import { PatientDashboard } from './Patients/patient-dashboard/patient-dashboard';
import { Appointments } from './Patients/appointments/appointments';
import { PatientDoctors } from './Patients/patient-doctors/patient-doctors';
import { HealthRecords } from './Patients/health-records/health-records';

import { DoctorDashboard } from './Doctors/d_doctor-dashboard/d_doctor-dashboard';
import { DoctorAppointments } from './Doctors/d_appointment/d_appointments';
import { DoctorHealthRecords } from './Doctors/d_health-records/d_health-records';

import { AuthGuard } from './guards/auth-guard';
import { RoleGuard } from './guards/role-guard';


export const routes: Routes = [

  { path: '', component: Home },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: Register },

  {
    path: 'patient-dashboard',
    component: PatientDashboard,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'User' }
  },
  {
    path: 'appointments',
    component: Appointments,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'User' }
  },
  {
    path: 'doctors',
    component: PatientDoctors,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'User' }
  },
  {
    path: 'health-records',
    component: HealthRecords,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'User' }
  },

  {
    path: 'doctor-dashboard',
    component: DoctorDashboard,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Doctor' }
  },
  {
    path: 'doctor_appointments',
    component: DoctorAppointments,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Doctor' }
  },
{
  path: 'd_leave',
  component: DLeave,
  canActivate: [AuthGuard, RoleGuard],
  data: { role: 'Doctor' }
},
  {
    path: 'doctor_health-records',
    component: DoctorHealthRecords,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Doctor' }
  },

  { path: '**', redirectTo: '' }
];