import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewHealthRecord } from './view-health-record';

describe('ViewHealthRecord', () => {
  let component: ViewHealthRecord;
  let fixture: ComponentFixture<ViewHealthRecord>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewHealthRecord],
    }).compileComponents();

    fixture = TestBed.createComponent(ViewHealthRecord);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
