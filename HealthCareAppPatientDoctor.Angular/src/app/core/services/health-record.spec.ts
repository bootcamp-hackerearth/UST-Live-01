import { TestBed } from '@angular/core/testing';

import { HealthRecord } from './health-record';

describe('HealthRecord', () => {
  let service: HealthRecord;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HealthRecord);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
