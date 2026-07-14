import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorLeave } from './doctor-leave';

describe('DoctorLeave', () => {
  let component: DoctorLeave;
  let fixture: ComponentFixture<DoctorLeave>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorLeave],
    }).compileComponents();

    fixture = TestBed.createComponent(DoctorLeave);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
