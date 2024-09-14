import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeatureCreateComponent } from './feature-create.component';

describe('FeatureCreateComponent', () => {
  let component: FeatureCreateComponent;
  let fixture: ComponentFixture<FeatureCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FeatureCreateComponent]
    });
    fixture = TestBed.createComponent(FeatureCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
