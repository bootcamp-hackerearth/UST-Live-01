import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { CreateAdminHandoffResponse } from '../models/admin-handoff.model';

@Injectable({
  providedIn: 'root'
})
export class AdminHandoffService {
  private readonly apiUrl = `${environment.apiBaseUrl}/admin-handoff`;

  constructor(private readonly httpClient: HttpClient) {
  }

  create(): Observable<CreateAdminHandoffResponse> {
    return this.httpClient.post<CreateAdminHandoffResponse>(
      `${this.apiUrl}/create`,
      {}
    );
  }
}
