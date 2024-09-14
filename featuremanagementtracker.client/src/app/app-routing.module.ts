import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeatureListComponent } from './feature-list/feature-list.component';
import { FeatureCreateComponent } from './feature-create/feature-create.component';
import { FeatureEditComponent } from './feature-edit/feature-edit.component';
import { FeatureDetailComponent } from './feature-detail/feature-detail.component';

const routes: Routes = [
  { path: 'feature-list', component: FeatureListComponent },
  { path: 'feature-create', component: FeatureCreateComponent },
  { path: 'feature-edit/:id', component: FeatureEditComponent },
  { path: 'feature-detail/:id', component: FeatureDetailComponent },
  { path: '', redirectTo: '/feature-list', pathMatch: 'full' },
  { path: '**', redirectTo: '/feature-list' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
