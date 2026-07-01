import { Routes } from '@angular/router';

import { Home } from './pages/home/home';
import { LoginComponent } from './pages/login/login';
import { RegisterComponent } from './pages/register/register';
import { PatientDashboardComponent } from './pages/patient-dashboard/patient-dashboard';
import { DoctorDashboardComponent } from './pages/doctor-dashboard/doctor-dashboard';
import { ChangePasswordComponent } from './pages/change-password/change-password';

import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    component: Home
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'patient',
    component: PatientDashboardComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: {
      roles: ['Patient']
    }
  },
  {
    path: 'doctor',
    component: DoctorDashboardComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: {
      roles: ['Doctor']
    }
  },
  {
    path: 'change-password',
    component: ChangePasswordComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: {
      roles: ['Doctor', 'Patient']
    }
  },
  {
    path: '**',
    redirectTo: ''
  }
];