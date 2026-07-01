import { TestBed } from '@angular/core/testing';

import { MockPatientData } from './mock-patient-data';

describe('MockPatientData', () => {
  let service: MockPatientData;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MockPatientData);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
