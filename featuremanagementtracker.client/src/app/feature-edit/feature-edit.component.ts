import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FeatureService } from '../feature.service';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';

@Component({
  selector: 'app-feature-edit',
  templateUrl: './feature-edit.component.html',
  styleUrls: ['./feature-edit.component.css']
})
export class FeatureEditComponent implements OnInit {
  featureForm: FormGroup;
  featureId!: number;
  isSubmitted = false;

  constructor(private fb: FormBuilder, private featureService: FeatureService, private router: Router, private route: ActivatedRoute) {
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

  ngOnInit(): void {
    const featureId = this.route.snapshot.paramMap.get('id');

    if (featureId) {
      const id = +featureId;
      if (id) {
        this.featureId = id;
        this.featureService.getFeature(this.featureId).subscribe(
          data => {
            if (data.targetDate) {
              data.targetDate = new Date(data.targetDate);
            }
            if (data.actualDate) {
              data.actualDate = new Date(data.actualDate);
            }
            this.featureForm.patchValue({
              ...data,
              targetDate: data.targetDate ? this.formatDate(data.targetDate) : '',
              actualDate: data.actualDate ? this.formatDate(data.actualDate) : ''
            });
            this.updateDateValidators(data.status);
          },
          error => console.error('Error fetching feature', error)
        );
      } else {
        console.error('Invalid ID parameter.');
      }
    } else {
      console.error('ID parameter is missing.');
    }
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
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

  private validateForm(): void {
    this.featureForm.markAllAsTouched();
    this.updateDateValidators(this.featureForm.get('status')?.value);
    this.featureForm.updateValueAndValidity();
  }

  onSubmit(): void {
    this.isSubmitted = true;
    this.validateForm();

    if (this.featureForm.valid) {
      this.featureForm.value.id = this.featureId;
      this.featureService.updateFeature(this.featureId, this.featureForm.value).subscribe(() => {
        this.router.navigate(['/feature-list']);
      });
    }
  }

  deleteFeature(): void {
    if (confirm('Are you sure you want to delete this feature?')) {
      this.featureService.deleteFeature(this.featureId).subscribe(() => this.router.navigate(['/feature-list']));
    }
  }
}
