import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FeatureService } from '../feature.service';
import { Router } from '@angular/router';
import { Feature } from '../FeatureModels/feature.model';

@Component({
  selector: 'app-feature-create',
  templateUrl: './feature-create.component.html',
  styleUrls: ['./feature-create.component.css']
})
export class FeatureCreateComponent {
  featureForm: FormGroup;
  isSubmitted = false;

  constructor(private fb: FormBuilder, private featureService: FeatureService, private router: Router) {
    this.featureForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      complexity: ['', Validators.required],
      status: ['', Validators.required],
      targetDate: [''],
      actualDate: [''],
    });

    this.featureForm.get('status')?.valueChanges.subscribe(status => {
      this.updateDateValidators(status);
    });
  }

  private updateDateValidators(status: string): void {
    const targetDateControl = this.featureForm.get('targetDate');
    const actualDateControl = this.featureForm.get('actualDate');

    if (status === 'active') {
      targetDateControl?.setValidators(Validators.required);
      actualDateControl?.clearValidators();
    } else if (status === 'closed') {
      actualDateControl?.setValidators(Validators.required);
      targetDateControl?.clearValidators();
    } else {
      targetDateControl?.clearValidators();
      actualDateControl?.clearValidators();
    }

    targetDateControl?.updateValueAndValidity();
    actualDateControl?.updateValueAndValidity();
  }

  onSubmit(): void {
    this.isSubmitted = true;

    if (this.featureForm.valid) {
      console.log('Form Data:', this.featureForm.value);
      this.featureService.createFeature(this.featureForm.value).subscribe(
        () => this.router.navigate(['/feature-list']));
    } else {
      console.log('Form is invalid');
    }
  }
}
