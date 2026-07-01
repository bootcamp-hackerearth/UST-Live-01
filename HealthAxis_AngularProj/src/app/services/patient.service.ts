import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class PatientService {

  private API = 'https://localhost:7054/api';

  constructor(private http: HttpClient) {}

  getPatient(id: string) {
    return this.http.get(`${this.API}/Patient/${id}`);
  }

  updatePatient(id: string, data: any) {
    return this.http.put(`${this.API}/Patient/${id}`, data);
  }

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

  /**
   * Loads doctors. For frontend filtering, we load enough records.
   * If your backend has a separate all-doctors endpoint, replace /Doctor/available accordingly.
   */
  getDoctors(
    pageNumber: number = 1,
    pageSize: number = 1000,
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

  bookAppointment(data: any) {
    return this.http.post(`${this.API}/Appointment`, data);
  }

  cancelAppointment(id: number) {
    return this.http.post(
      `${this.API}/Appointment/cancel/${id}?reason=Cancelled by patient`,
      {},
      { responseType: 'text' }
    );
  }
}