import { Routes } from '@angular/router';
import { Home } from './home/home';
import { LoginComponent } from './login/login-request.model';
import { Register } from './register/register';
import { PatientDashboard } from './Patients/patient-dashboard/patient-dashboard';
import { Appointments } from './Patients/appointments/appointments';
import { PatientDoctors } from './Patients/patient-doctors/patient-doctors';
import { HealthRecords } from './Patients/health-records/health-records';
import { DoctorDashboard } from './Doctors/d_doctor-dashboard/d_doctor-dashboard';
import { DoctorHealthRecords } from './Doctors/d_health-records/d_health-records';
import { DoctorAppointments } from './Doctors/d_appointment/d_appointments';



export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: Register },

  { path: 'patient-dashboard', component: PatientDashboard },
  { path: 'appointments', component: Appointments },
  { path: 'doctors', component: PatientDoctors },
  { path: 'health-records', component: HealthRecords },


  { path: 'doctor-dashboard', component: DoctorDashboard },
  { path: 'doctor_appointments', component: DoctorAppointments },
  { path: 'doctor_health-records', component: DoctorHealthRecords },


  { path: '**', redirectTo: '' }
];