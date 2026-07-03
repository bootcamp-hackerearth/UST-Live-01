import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateHealthRecord } from './create-health-record';

describe('CreateHealthRecord', () => {
  let component: CreateHealthRecord;
  let fixture: ComponentFixture<CreateHealthRecord>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateHealthRecord],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateHealthRecord);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
