import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { FeatureListComponent } from './feature-list/feature-list.component';
import { FeatureCreateComponent } from './feature-create/feature-create.component';
import { FeatureEditComponent } from './feature-edit/feature-edit.component';
import { FeatureDetailComponent } from './feature-detail/feature-detail.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    FeatureListComponent,
    FeatureCreateComponent,
    FeatureEditComponent,
    FeatureDetailComponent,
    ConfirmDialogComponent    
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
