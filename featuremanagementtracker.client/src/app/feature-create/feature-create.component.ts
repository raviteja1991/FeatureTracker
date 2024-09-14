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

  constructor(private fb: FormBuilder, private featureService: FeatureService, private router: Router) {
    this.featureForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      complexity: [''],
      status: [''],
      targetDate: [''],
      actualDate: [''],
    });
  }

  onSubmit(): void {
    if (this.featureForm.valid) {
      this.featureService.createFeature(this.featureForm.value).subscribe(() => {
        this.router.navigate(['/feature-list']);
      });
    }
  }
}
