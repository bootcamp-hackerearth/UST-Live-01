import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

import { LandingComponent } from './features/auth/landing/landing';

import { PatientLayoutComponent } from './shared/layouts/patient-layout/patient-layout';
import { DoctorLayoutComponent } from './shared/layouts/doctor-layout/doctor-layout';

import { DashboardComponent as PatientDashboard }from './features/patient/dashboard/dashboard';

import { BookAppointmentComponent } from './features/patient/book-appointment/book-appointment';
import { MyAppointmentsComponent } from './features/patient/my-appointments/my-appointments';
import { ProfileComponent } from './features/patient/profile/profile';
import { ScheduleComponent } from './features/doctor/schedule/schedule';
import { LeaveComponent } from './features/doctor/leave/leave';
import { DoctorProfileComponent } from './features/doctor/profile/profile';
import { DoctorDashboardComponent } from './features/doctor/dashboard/dashboard';
import { HealthRecordComponent } from './features/doctor/health-record/health-record';
import { HealthHistoryComponent } from './features/patient/health-history/health-history';


export const routes: Routes = [ 

  {
    path: '',
    component: LandingComponent
  },

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
      },

      {
         path: 'my-appointments',
        component: MyAppointmentsComponent
      },
      {
         path:'profile',
        component:ProfileComponent,
      },
      {
         path:'book-appointment',
        component: BookAppointmentComponent,
      },

      {
        path: 'health-history',
        component: HealthHistoryComponent
      }
    ]
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
        component: DoctorDashboardComponent
      },
       {
          path: 'leave',
          component: LeaveComponent,
       },

        {
          path: 'doctor/schedule',
          component: ScheduleComponent,
        },

        {
        path: 'profile',
        component: DoctorProfileComponent,
        },

        {
        path: 'health-record',
        component: HealthRecordComponent,
        }

    ]
  },
];
