import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AboutComponent } from './about/about.component';
import { ToursComponent } from './tours/tours.component';
import { TourService } from './tours/shared/tour.service';
import { TourUpdateComponent } from './tours/tour-update/tour-update.component';
import { TourAddComponent } from './tours/tour-add/tour-add.component';
import { TourDetailComponent } from './tours/tour-detail/tour-detail.component';
import { ShowsComponent } from './tours/shows/shows.component';
import { MasterDataService } from './tours/shared/master-data.service';
import { ShowAddComponent } from './tours/shows/show-add/show-add.component';
import { ShowService } from './tours/shared/show.service';


@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    ToursComponent,
    TourUpdateComponent,
    TourAddComponent,
    TourDetailComponent,
    ShowsComponent,
    ShowAddComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  providers: [ TourService, MasterDataService, ShowService],
  bootstrap: [AppComponent]
})
export class AppModule { }
