import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodaySchedule } from './today-schedule';

describe('TodaySchedule', () => {
  let component: TodaySchedule;
  let fixture: ComponentFixture<TodaySchedule>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TodaySchedule],
    }).compileComponents();

    fixture = TestBed.createComponent(TodaySchedule);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
