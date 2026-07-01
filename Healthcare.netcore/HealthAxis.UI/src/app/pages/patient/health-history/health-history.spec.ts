import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HealthHistory } from './health-history';

describe('HealthHistory', () => {
  let component: HealthHistory;
  let fixture: ComponentFixture<HealthHistory>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HealthHistory],
    }).compileComponents();

    fixture = TestBed.createComponent(HealthHistory);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
