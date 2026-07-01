import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class DoctorService {

  private API = 'https://localhost:7054/api';

  constructor(private http: HttpClient) {}

  getDoctor(id: string) {
    return this.http.get(`${this.API}/Doctor/${id}`);
  }

  getDoctorAppointments(
    doctorId: string,
    pageNumber: number = 1,
    pageSize: number = 10,
    search: string = '',
    status: string = ''
  ) {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (search) {
      params = params.set('search', search);
    }

    if (status) {
      params = params.set('status', status);
    }

    return this.http.get(`${this.API}/Appointment/doctor/${doctorId}`, { params });
  }

  confirmAppointment(appointmentId: number) {
    return this.http.post(
      `${this.API}/Appointment/confirm/${appointmentId}`,
      {},
      { responseType: 'text' }
    );
  }

  completeAppointment(appointmentId: number) {
    return this.http.post(
      `${this.API}/Appointment/complete/${appointmentId}`,
      {},
      { responseType: 'text' }
    );
  }

  cancelAppointment(appointmentId: number, reason: string) {
    return this.http.post(
      `${this.API}/Appointment/cancel/${appointmentId}?reason=${encodeURIComponent(reason)}`,
      {},
      { responseType: 'text' }
    );
  }

  getPatient(patientId: number) {
    return this.http.get(`${this.API}/Patient/${patientId}`);
  }

  getPatientHealthRecords(
    patientId: number,
    pageNumber: number = 1,
    pageSize: number = 10
  ) {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get(`${this.API}/HealthRecord/patient/${patientId}`, { params });
  }

  createHealthRecord(data: any) {
    return this.http.post(`${this.API}/HealthRecord`, data);
  }
}