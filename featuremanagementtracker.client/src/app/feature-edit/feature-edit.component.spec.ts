import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeatureEditComponent } from './feature-edit.component';

describe('FeatureEditComponent', () => {
  let component: FeatureEditComponent;
  let fixture: ComponentFixture<FeatureEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FeatureEditComponent]
    });
    fixture = TestBed.createComponent(FeatureEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
