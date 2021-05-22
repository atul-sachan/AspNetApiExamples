import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
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
import { ShowSingleComponent } from './tours/shows/show-single/show-single.component';

import 'automapper-ts';
import { EnsureAcceptHeaderInterceptor } from './tours/shared/ensure-accept-header.Interceptor';
import { GlobalErrorHandler } from './tours/shared/global-error-handler';
import { ErrorLoggerService } from './tours/shared/error-logger.service';



@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    ToursComponent,
    TourUpdateComponent,
    TourAddComponent,
    TourDetailComponent,
    ShowsComponent,
    ShowAddComponent,
    ShowSingleComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  providers: [ 
    // { provide: HTTP_INTERCEPTORS, useClass: EnsureAcceptHeaderInterceptor, multi: true},
    GlobalErrorHandler, 
    ErrorLoggerService, 
    TourService, 
    MasterDataService, 
    ShowService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

  constructor(){
    // automapper mappings

    automapper.createMap('TourFormModel', 'TourForCreation')
      .forSourceMember('band', (opts: AutoMapperJs.ISourceMemberConfigurationOptions) => { opts.ignore(); })
      .forSourceMember('manager', (opts: AutoMapperJs.ISourceMemberConfigurationOptions) => { opts.ignore(); })
      .forMember('bandid', function (opts) { opts.mapFrom('band'); });

    automapper.createMap('TourFormModel', 'TourWithManagerForCreation')
      .forSourceMember('band', (opts: AutoMapperJs.ISourceMemberConfigurationOptions) => { opts.ignore(); })
      .forSourceMember('manager', (opts: AutoMapperJs.ISourceMemberConfigurationOptions) => { opts.ignore(); })
      .forMember('bandid', function (opts) { opts.mapFrom('band'); })
      .forMember('managerid', function (opts) { opts.mapFrom('manager'); })

    automapper.createMap('TourFormModel', 'TourWithShowsForCreation')
      .forSourceMember('band', (opts: AutoMapperJs.ISourceMemberConfigurationOptions) => { opts.ignore(); })
      .forSourceMember('manager', (opts: AutoMapperJs.ISourceMemberConfigurationOptions) => { opts.ignore(); })
      .forMember('bandid', function (opts) { opts.mapFrom('band'); });

    automapper.createMap('TourFormModel', 'TourWithManagerAndShowsForCreation')
      .forSourceMember('band', (opts: AutoMapperJs.ISourceMemberConfigurationOptions) => 
      { opts.ignore(); })
      .forSourceMember('manager', (opts: AutoMapperJs.ISourceMemberConfigurationOptions) => 
      { opts.ignore(); })
      .forMember('bandid', function (opts) { opts.mapFrom('band'); })
      .forMember('managerid', function (opts) { opts.mapFrom('manager'); })

      automapper.createMap('ShowCollectionFormModelShowsArray', 
      'ShowCollectionForCreation');

      automapper.createMap('TourFormModel', 'TourForUpdate');
  }

 }
