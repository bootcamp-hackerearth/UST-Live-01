import { Injectable, inject } from '@angular/core';
import {
  MockPatientData,
  PatientDto,
  RegisterPatientRequest
} from './mock-patient-data';

export type UserRole = 'Patient' | 'Doctor' | 'Admin';

export interface MockUser {
  userId: string;
  email: string;
  password: string;
  role: UserRole;
  fullName: string;
  patientId?: number;
  doctorId?: number;
  mustChangePassword?: boolean;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResult {
  success: boolean;
  message: string;
  user?: MockUser;
}

export interface RegisterResult {
  success: boolean;
  message: string;
  patient?: PatientDto;
}

@Injectable({
  providedIn: 'root'
})
export class MockAuth {
  private patientData = inject(MockPatientData);

  private users: MockUser[] = [
    {
      userId: 'patient-user-1',
      email: 'kevin@gmail.com',
      password: 'Kevin@1234',
      role: 'Patient',
      fullName: 'Kevin',
      patientId: 1
    },
    {
      userId: 'patient-user-2',
      email: 'anu@gmail.com',
      password: 'Anu@1234',
      role: 'Patient',
      fullName: 'Anu Joseph',
      patientId: 2
    },
    {
      userId: 'patient-user-3',
      email: 'rahul@gmail.com',
      password: 'Rahul@1234',
      role: 'Patient',
      fullName: 'Rahul Nair',
      patientId: 3
    },
    {
      userId: 'doctor-user-1',
      email: 'vignesh@gmail.com',
      password: 'Vignesh@1234',
      role: 'Doctor',
      fullName: 'Dr Vignesh Kumar',
      doctorId: 1,
      mustChangePassword: false
    },
    {
      userId: 'doctor-user-2',
      email: 'anjali@gmail.com',
      password: 'Anjali@123',
      role: 'Doctor',
      fullName: 'Dr Anjali Menon',
      doctorId: 2,
      mustChangePassword: true
    },
    {
      userId: 'doctor-user-3',
      email: 'arjun@gmail.com',
      password: 'Arjun@123',
      role: 'Doctor',
      fullName: 'Dr Arjun Nair',
      doctorId: 3,
      mustChangePassword: false
    },
    {
      userId: 'admin-user-1',
      email: 'admin@healthapp.com',
      password: 'Admin@123',
      role: 'Admin',
      fullName: 'Admin User'
    }
  ];

  private currentUser?: MockUser;

  login(request: LoginRequest): LoginResult {
    const email = request.email.trim().toLowerCase();

    if (!email || !request.password) {
      return {
        success: false,
        message: 'Please enter email and password.'
      };
    }

    const user = this.users.find(
      item =>
        item.email.toLowerCase() === email &&
        item.password === request.password
    );

    if (!user) {
      return {
        success: false,
        message: 'Invalid email or password.'
      };
    }

    this.currentUser = user;

    return {
      success: true,
      message: 'Login successful.',
      user
    };
  }

  registerPatient(request: RegisterPatientRequest): RegisterResult {
    const existingUser = this.users.find(
      user => user.email.toLowerCase() === request.email.trim().toLowerCase()
    );

    if (existingUser) {
      return {
        success: false,
        message: 'A user with this email already exists.'
      };
    }

    const result = this.patientData.addPatient(request);

    if (!result.success || !result.data) {
      return {
        success: false,
        message: result.message
      };
    }

    const patient = result.data;

    this.users.push({
      userId: `patient-user-${patient.patientId}`,
      email: patient.email ?? '',
      password: request.password,
      role: 'Patient',
      fullName: patient.fullName,
      patientId: patient.patientId
    });

    return {
      success: true,
      message: 'Patient registered successfully. You can login now.',
      patient
    };
  }

  logout(): void {
    this.currentUser = undefined;
  }

  getCurrentUser(): MockUser | undefined {
    return this.currentUser;
  }

  getCurrentPatient(): PatientDto | undefined {
    if (!this.currentUser?.patientId) {
      return undefined;
    }

    return this.patientData.getPatientById(this.currentUser.patientId);
  }

  isLoggedIn(): boolean {
    return !!this.currentUser;
  }

  isPatient(): boolean {
    return this.currentUser?.role === 'Patient';
  }
  syncCurrentUserFromPatient(patient: PatientDto): void {
  const user = this.users.find(item => item.patientId === patient.patientId);

  if (!user) {
    return;
  }

  user.fullName = patient.fullName;
  user.email = patient.email ?? user.email;

  if (this.currentUser?.patientId === patient.patientId) {
    this.currentUser.fullName = user.fullName;
    this.currentUser.email = user.email;
  }
}

changeCurrentUserPassword(
  currentPassword: string,
  newPassword: string,
  confirmPassword: string
): LoginResult {
  if (!this.currentUser) {
    return {
      success: false,
      message: 'Please login again.'
    };
  }

  if (!currentPassword || !newPassword || !confirmPassword) {
    return {
      success: false,
      message: 'Please fill all password fields.'
    };
  }

  if (this.currentUser.password !== currentPassword) {
    return {
      success: false,
      message: 'Current password is incorrect.'
    };
  }

  if (newPassword.length < 6) {
    return {
      success: false,
      message: 'New password must be at least 6 characters long.'
    };
  }

  if (newPassword !== confirmPassword) {
    return {
      success: false,
      message: 'New password and confirm password do not match.'
    };
  }

  this.currentUser.password = newPassword;

  const user = this.users.find(item => item.userId === this.currentUser?.userId);

  if (user) {
    user.password = newPassword;
  }

  return {
    success: true,
    message: 'Password changed successfully.',
    user: this.currentUser
  };
}
changeTemporaryPassword(
  currentPassword: string,
  newPassword: string,
  confirmPassword: string
): LoginResult {
  if (!this.currentUser) {
    return {
      success: false,
      message: 'Please login again before changing password.'
    };
  }

  if (!this.currentUser.mustChangePassword) {
    return {
      success: false,
      message: 'Password change is not required for this account.'
    };
  }

  if (!currentPassword || !newPassword || !confirmPassword) {
    return {
      success: false,
      message: 'Please fill all password fields.'
    };
  }

  if (this.currentUser.password !== currentPassword) {
    return {
      success: false,
      message: 'Current temporary password is incorrect.'
    };
  }

  if (newPassword.length < 6) {
    return {
      success: false,
      message: 'New password must be at least 6 characters long.'
    };
  }

  if (newPassword !== confirmPassword) {
    return {
      success: false,
      message: 'New password and confirm password do not match.'
    };
  }

  const user = this.users.find(item => item.userId === this.currentUser?.userId);

  if (!user) {
    return {
      success: false,
      message: 'User account was not found.'
    };
  }

  user.password = newPassword;
  user.mustChangePassword = false;

  this.currentUser.password = newPassword;
  this.currentUser.mustChangePassword = false;

  return {
    success: true,
    message: 'Password changed successfully. Please login again.',
    user: this.currentUser
  };
}
}