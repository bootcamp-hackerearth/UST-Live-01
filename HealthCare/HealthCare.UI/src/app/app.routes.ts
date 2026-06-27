import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

import { LandingComponent } from './features/auth/landing/landing';

import { PatientLayoutComponent } from './shared/layouts/patient-layout/patient-layout';
import { DoctorLayoutComponent } from './shared/layouts/doctor-layout/doctor-layout';

import { DashboardComponent as PatientDashboard }
from './features/patient/dashboard/dashboard';

import { DashboardComponent as DoctorDashboard }
from './features/doctor/dashboard/dashboard';
import { BookAppointmentComponent } from './features/patient/book-appointment/book-appointment';


export const routes: Routes = [ 

  {
    path: '',
    component: LandingComponent
  },

//   {
//     path: 'login',
//     component: LoginComponent
//   },

//   {
//     path: 'register',
//     component: RegisterComponent
//   },

  // PATIENT

  {
    path: 'patient',
    component: PatientLayoutComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Patient'] },

    children: [

      {
        path: 'dashboard',
        component: PatientDashboard
      }

    ]
  },

  {
  path:'patient/book-appointment',
  component: BookAppointmentComponent,
  canActivate:[authGuard, roleGuard],
  data:{roles:['Patient']}
},

  // DOCTOR

  {
    path: 'doctor',
    component: DoctorLayoutComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Doctor'] },

    children: [

      {
        path: 'dashboard',
        component: DoctorDashboard
      }

    ]
  }

];
