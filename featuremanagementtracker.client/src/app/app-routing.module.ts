import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
//import { HomeComponent } from './home/home.component';
//import { AboutComponent } from './about/about.component';

const routes: Routes = [
  //{ path: '', component: HomeComponent },
  //{ path: 'about', component: AboutComponent },
  // Add more routes here
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
