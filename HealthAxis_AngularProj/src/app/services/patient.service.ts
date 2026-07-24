import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class PatientService {

  private API = '/api';

  constructor(private http: HttpClient) {}

  // ================= PATIENT =================

  getPatient(id: string) {
    return this.http.get(`${this.API}/Patient/${id}`);
  }

  updatePatient(id: string, data: any) {
    return this.http.put(
      `${this.API}/Patient/${id}`,
      data,
      { responseType: 'text' }
    );
  }

  // ================= DOCTORS =================

  getDoctors(
    pageNumber: number = 1,
    pageSize: number = 10,
    search: string = '',
    specialisation: string = '',
    status: string = ''
  ) {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (search) {
      params = params.set('search', search);
    }

    if (specialisation) {
      params = params.set('specialisation', specialisation);
    }

    if (status) {
      params = params.set('status', status);
    }

    return this.http.get(`${this.API}/Doctor/available`, { params });
  }

  // ================= APPOINTMENTS =================

  getAppointments(
    patientId: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get(`${this.API}/Appointment/patient/${patientId}`, { params });
  }

  bookAppointment(data: any) {
    return this.http.post(`${this.API}/Appointment`, data);
  }

  cancelAppointment(appointmentId: number) {
    return this.http.post(
      `${this.API}/Appointment/cancel/${appointmentId}?reason=${encodeURIComponent('Cancelled by patient')}`,
      {},
      { responseType: 'text' }
    );
  }

  getBookedSlots(doctorId: number, date: string) {
    const params = new HttpParams()
      .set('doctorId', doctorId)
      .set('date', date);

    return this.http.get<string[]>(`${this.API}/Appointment/booked-slots`, { params });
  }

  // ================= HEALTH RECORDS =================

  getHealthRecords(
    patientId: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ) {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get(`${this.API}/HealthRecord/patient/${patientId}`, { params });
  }
}