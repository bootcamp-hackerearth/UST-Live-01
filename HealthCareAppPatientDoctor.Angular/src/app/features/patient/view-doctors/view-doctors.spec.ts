import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDoctors } from './view-doctors';

describe('ViewDoctors', () => {
  let component: ViewDoctors;
  let fixture: ComponentFixture<ViewDoctors>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewDoctors],
    }).compileComponents();

    fixture = TestBed.createComponent(ViewDoctors);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
