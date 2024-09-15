import { Component, OnInit } from '@angular/core';
import { FeatureService } from '../feature.service';
import { MatDialog } from '@angular/material/dialog';
import { Feature } from '../FeatureModels/feature.model';
import { Router } from '@angular/router';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-feature-list',
  templateUrl: './feature-list.component.html',
  styleUrls: ['./feature-list.component.css']
})
export class FeatureListComponent implements OnInit {

  features: Feature[] = [];
  successMessage: string | null = null;

  constructor(private featureService: FeatureService, private router: Router, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadFeatures(true);
  }

  loadFeatures(resetMsg: boolean = false): void {
    this.featureService.getFeatures().subscribe((data: Feature[]) => {
      this.features = data;
    });
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
      const dialogRef = this.dialog.open(ConfirmDialogComponent);

      dialogRef.afterClosed().subscribe(result => {
        if (result == true) {
          this.featureService.deleteFeature(id).subscribe(() => {
            this.loadFeatures();
            this.successMessage = 'Feature deleted successfully.';
          //  this.autoCloseMessage();
          });
        }
      });
    } else {
      console.error('Feature ID is undefined.');
    }
  }

  closeMessage(): void {
    this.successMessage = null;
  }

  autoCloseMessage(): void {
    setTimeout(() => {
      this.successMessage = null;
    }, 5000);
  }
}
