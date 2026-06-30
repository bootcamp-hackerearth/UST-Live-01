import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorPatientProfile } from './doctor-patient-profile';

describe('DoctorPatientProfile', () => {
  let component: DoctorPatientProfile;
  let fixture: ComponentFixture<DoctorPatientProfile>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorPatientProfile],
    }).compileComponents();

    fixture = TestBed.createComponent(DoctorPatientProfile);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
