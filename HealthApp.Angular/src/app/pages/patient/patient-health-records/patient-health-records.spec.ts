import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientHealthRecords } from './patient-health-records';

describe('PatientHealthRecords', () => {
  let component: PatientHealthRecords;
  let fixture: ComponentFixture<PatientHealthRecords>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientHealthRecords],
    }).compileComponents();

    fixture = TestBed.createComponent(PatientHealthRecords);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
