import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeatureDetailComponent } from './feature-detail.component';

describe('FeatureDetailComponent', () => {
  let component: FeatureDetailComponent;
  let fixture: ComponentFixture<FeatureDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FeatureDetailComponent]
    });
    fixture = TestBed.createComponent(FeatureDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
