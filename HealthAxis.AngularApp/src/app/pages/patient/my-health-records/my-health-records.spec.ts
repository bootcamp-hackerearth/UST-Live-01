import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyHealthRecords } from './my-health-records';

describe('MyHealthRecords', () => {
  let component: MyHealthRecords;
  let fixture: ComponentFixture<MyHealthRecords>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MyHealthRecords],
    }).compileComponents();

    fixture = TestBed.createComponent(MyHealthRecords);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
