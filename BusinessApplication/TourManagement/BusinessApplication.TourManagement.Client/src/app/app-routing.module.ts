import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { ShowAddComponent } from './tours/shows/show-add/show-add.component';
import { TourAddComponent } from './tours/tour-add/tour-add.component';
import { TourDetailComponent } from './tours/tour-detail/tour-detail.component';
import { TourUpdateComponent } from './tours/tour-update/tour-update.component';
import { ToursComponent } from './tours/tours.component';

const routes: Routes = [
  { path: '', redirectTo: 'tours', pathMatch: 'full' },
    { path: 'tours', component: ToursComponent },
    { path: 'about', component: AboutComponent },
    { path: 'tours/:tourId', component: TourDetailComponent },
    { path: 'tour-update/:tourId', component: TourUpdateComponent },  
    { path: 'tour-add', component: TourAddComponent },  
    { path: 'tours/:tourId/show-add', component: ShowAddComponent }
  //  { path: '**', redirectTo: 'tours' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
