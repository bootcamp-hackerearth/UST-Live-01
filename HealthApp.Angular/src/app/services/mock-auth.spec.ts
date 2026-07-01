import { TestBed } from '@angular/core/testing';

import { MockAuth } from './mock-auth';

describe('MockAuth', () => {
  let service: MockAuth;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MockAuth);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
