import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DoctorStatusStateService {
  private readonly doctorActiveStatus = signal<boolean | null>(null);

  readonly isActive = this.doctorActiveStatus.asReadonly();

  setStatus(isActive: boolean): void {
    this.doctorActiveStatus.set(isActive);
  }

  clearStatus(): void {
    this.doctorActiveStatus.set(null);
  }
}