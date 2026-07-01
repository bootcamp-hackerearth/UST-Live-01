import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorChangePasswordRequired } from './doctor-change-password-required';

describe('DoctorChangePasswordRequired', () => {
  let component: DoctorChangePasswordRequired;
  let fixture: ComponentFixture<DoctorChangePasswordRequired>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorChangePasswordRequired],
    }).compileComponents();

    fixture = TestBed.createComponent(DoctorChangePasswordRequired);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
