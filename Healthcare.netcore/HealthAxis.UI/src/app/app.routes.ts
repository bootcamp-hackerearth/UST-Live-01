import { Landing } from './pages/landing/landing';
import { Login } from './pages/auth/login/login';
import {Register} from './pages/auth/register/register';

//  PATIENT IMPORTS
import { Dashboard } from './pages/patient/dashboard/dashboard';
import { DoctorSearch } from './pages/patient/doctor-search/doctor-search';
import { MyAppointments } from './pages/patient/my-appointments/my-appointments';
import { HealthHistory } from './pages/patient/health-history/health-history';
import { BookAppointment } from './pages/patient/book-appointment/book-appointment';
import { PatientLayout } from './layout/patient-layout/patient-layout';
import { authGuard } from './auth-guard';
import { Profile } from './pages/patient/profile/profile';

//  DOCTOR IMPORTS
import { DoctorLayout } from './layout/doctor-layout/doctor-layout';
import { Dashboard as DoctorDashboard } from './pages/doctor/dashboard/dashboard';
import { Appointments } from './pages/doctor/appointments/appointments';
import { AddRecord } from './pages/doctor/add-record/add-record';
import { DoctorProfile } from './pages/doctor/profile/profile';


export const routes = [
  //  ROOT
  { path: '', component: Landing },

  //  LOGIN
  { path: 'login', component: Login },
  { path: 'register', component: Register },

  //  PATIENT MODULE
  {
    path: 'patient',
    component: PatientLayout,
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', component: Dashboard },
      { path: 'search', component: DoctorSearch,runGuardsAndResolvers:'always' as const },
      { path: 'appointments', component: MyAppointments},
      { path: 'history', component: HealthHistory },
      { path: 'book', component: BookAppointment },
      { path: 'profile', component: Profile },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' as const },
    ]
  },

  //  DOCTOR MODULE 
  {
    path: 'doctor',
    component: DoctorLayout,
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', component: DoctorDashboard },
      { path: 'appointments', component: Appointments },
      { path: 'add-record', component: AddRecord },
      { path: 'profile', component: DoctorProfile },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' as const }
    ]
  }
];