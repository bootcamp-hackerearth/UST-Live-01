import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddHealthRecord } from './add-health-record';

describe('AddHealthRecord', () => {
  let component: AddHealthRecord;
  let fixture: ComponentFixture<AddHealthRecord>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddHealthRecord],
    }).compileComponents();

    fixture = TestBed.createComponent(AddHealthRecord);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
