import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PatientService } from '../Patient.service/patientservice';

@Injectable({
  providedIn: 'root'
})
export class doctorPatientService {

  private baseUrl = 'https://localhost:7066/api/doctors';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {

    const token = localStorage.getItem('token');
    
    
  return new HttpHeaders({
    Authorization: `Bearer ${token}`
  });

  }}