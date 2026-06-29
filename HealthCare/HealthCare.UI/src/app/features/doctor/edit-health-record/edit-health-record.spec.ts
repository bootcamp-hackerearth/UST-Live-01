import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditHealthRecord } from './edit-health-record';

describe('EditHealthRecord', () => {
  let component: EditHealthRecord;
  let fixture: ComponentFixture<EditHealthRecord>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditHealthRecord],
    }).compileComponents();

    fixture = TestBed.createComponent(EditHealthRecord);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
