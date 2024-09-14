import { Component, OnInit } from '@angular/core';
import { FeatureService } from '../feature.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { Feature } from '../FeatureModels/feature.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-feature-list',
  templateUrl: './feature-list.component.html',
  styleUrls: ['./feature-list.component.css']
})
export class FeatureListComponent implements OnInit {

  features: Feature[] = [];

  constructor(private featureService: FeatureService, private router: Router) { }

  ngOnInit(): void {
    this.loadFeatures();
  }

  loadFeatures(): void {
    this.featureService.getFeatures().subscribe((data: Feature[]) => this.features = data);
  }

  navigateToCreate(): void {
    this.router.navigate(['/feature-create']);
  }

  navigateToEdit(id: number | undefined): void {
    if (id !== undefined) {
      this.router.navigate(['/feature-edit', id]);
    } else {
      console.error('Feature ID is undefined.');
    }
  }

  deleteFeature(id: number | undefined): void {
    if (id !== undefined) {
      if (confirm('Are you sure you want to delete this feature?')) {
        this.featureService.deleteFeature(id).subscribe(() => this.loadFeatures());
      }
    } else {
      console.error('Feature ID is undefined.');
    }
  }
}
