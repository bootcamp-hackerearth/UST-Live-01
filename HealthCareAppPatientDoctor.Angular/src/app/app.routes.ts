import { Routes } from '@angular/router';

import { Landing } from './features/home/landing/landing';
import { Login } from './features/auth/login/login';
import { PatientDashboard } from './features/patient/patient-dashboard/patient-dashboard';
import { BookAppointment } from './features/patient/book-appointment/book-appointment';
import { ViewAppointments } from './features/patient/view-appointments/view-appointments';
import { ViewDoctors } from './features/patient/view-doctors/view-doctors';
import { ViewHealthRecord } from './features/patient/view-health-record/view-health-record';
import { ReportView } from './features/patient/report-view/report-view';
import { NotFound } from './shared/components/not-found/not-found';
import { Register } from './features/auth/register/register';
import { ChangePassword } from './features/auth/change-password/change-password';
import { DoctorDashboard } from './features/doctor/doctor-dashboard/doctor-dashboard';

import { authGuard } from './core/guards/auth-guard';
import { DoctorAppointments } from './features/doctor/doctor-appointments/doctor-appointments';
import { DoctorAddHealthRecord } from './features/doctor/doctor-add-health-record/doctor-add-health-record';
import { DoctorPatientHistory } from './features/doctor/doctor-patient-history/doctor-patient-history';
import { DoctorProfile } from './features/doctor/doctor-profile/doctor-profile';
import { PatientProfileComponent } from './features/patient/patient-profile/patient-profile';
import { PatientEditProfile } from './features/patient/patient-edit-profile/patient-edit-profile';
import { PatientHealthRecords } from './features/patient/patient-health-records/patient-health-records';

export const routes: Routes = [

    {
        path: '',
        component: Landing
    },

    {
        path: 'login',
        component: Login
    },

    {
        path: 'patient',
        component: PatientDashboard,
        canActivate: [authGuard]
    },

    {
        path: 'book-appointment',
        component: BookAppointment,
        canActivate: [authGuard]
    },

    {
        path: 'appointments',
        component: ViewAppointments,
        canActivate: [authGuard]
    },

    {
        path: 'doctors',
        component: ViewDoctors,
        canActivate: [authGuard]
    },

    {
        path: 'health-records',
        component: ViewHealthRecord,
        canActivate: [authGuard]
    },

    {
        path: 'report/:id',
        component: ReportView,
        canActivate: [authGuard]
    },
    {
        path: 'register',
        component: Register
    },
    {
        path: 'doctor/change-password',
        component: ChangePassword
    },

    {
        path: 'doctor',
        component: DoctorDashboard,
        canActivate: [authGuard]
    },

    {
        path: 'doctor/appointments',
        component: DoctorAppointments,
        canActivate: [authGuard]
    },
    {
        path: 'doctor/appointments/:appointmentId/health-record',
        component: DoctorAddHealthRecord,
        canActivate: [authGuard]
    },


    {
        path: 'doctor/appointments/:appointmentId/history',
        component: DoctorPatientHistory,
        canActivate: [authGuard]
    },
    {
    path: 'doctor/profile',
    component: DoctorProfile,
    canActivate: [authGuard]
},

{
    path: 'patient/profile',
    component: PatientProfileComponent,
    canActivate: [authGuard]
},
{
    path: 'patient/profile/edit',
    component: PatientEditProfile,
    canActivate: [authGuard]
},
{
    path:'patient/health-records',
    component:PatientHealthRecords,
    canActivate:[authGuard]
},

    {
        path: '**',
        component: NotFound
    }



];