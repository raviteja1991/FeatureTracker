import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent {
  constructor(public dialogRef: MatDialogRef<ConfirmDialogComponent>) { }

  confirm(): void {
    this.dialogRef.close(true);
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
