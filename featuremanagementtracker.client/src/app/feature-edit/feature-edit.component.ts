import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FeatureService } from '../feature.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-feature-edit',
  templateUrl: './feature-edit.component.html',
  styleUrls: ['./feature-edit.component.css']
})
export class FeatureEditComponent implements OnInit {
  featureForm: FormGroup;
  featureId!: number;

  constructor(private fb: FormBuilder, private featureService: FeatureService, private router: Router, private route: ActivatedRoute) {
    this.featureForm = this.fb.group({
      title: [''],
      description: [''],
      complexity: [''],
      status: [''],
      targetDate: [''],
      actualDate: ['']
    });
  }

  ngOnInit(): void {
    const featureId = this.route.snapshot.paramMap.get('id');

    if (featureId) {
      const id = +featureId;
      if (id) {
        this.featureId = id;
        this.featureService.getFeature(this.featureId).subscribe(
          data => this.featureForm.patchValue(data),
          error => console.error('Error fetching feature', error)
        );
      } else {
        console.error('Invalid ID parameter.');
      }
    } else {
      console.error('ID parameter is missing.');
    }
  }

  onSubmit(): void {
    if (this.featureForm.valid) {
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
