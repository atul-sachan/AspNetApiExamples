import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Band } from '../shared/band.model';
import { CustomValidators } from '../shared/custom-validators';
import { Manager } from '../shared/manager.model';
import { MasterDataService } from '../shared/master-data.service';
import { TourService } from '../shared/tour.service';
import { ValidationErrorHandler } from '../shared/validation-error-handler';
import { ShowSingleComponent } from '../shows/show-single/show-single.component';

@Component({
  selector: 'app-tour-add',
  templateUrl: './tour-add.component.html',
  styleUrls: ['./tour-add.component.scss']
})
export class TourAddComponent implements OnInit {

  public tourForm: FormGroup;
  bands: Band[];
  isAdmin = true;
  managers: Manager[];

  constructor(private masterDataService: MasterDataService, private tourService: TourService, private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.tourForm = this.formBuilder.group({
      band: ['', Validators.required],
      manager: [''],
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', Validators.maxLength(2000)],
      startDate: [, Validators.required],
      endDate: [, Validators.required],
      shows: this.formBuilder.array([])
    }, { validator: CustomValidators.StartDateBeforeEndDateValidator });

    // get bands from master data service
    this.masterDataService.getBands()
      .subscribe(bands => {
        this.bands = bands;
      });

    if (this.isAdmin === true) {
      // get managers from master data service
      this.masterDataService.getManagers()
        .subscribe(managers => {
          this.managers = managers;
        });
    }
  }

  addShow(): void {
    let showsFormArray = this.tourForm.get('shows') as FormArray;
    // add show 
    showsFormArray.push(ShowSingleComponent.createShow());
  }
  
  addTour(): void {
    if (this.tourForm.dirty && this.tourForm.valid) {
      if (this.isAdmin === true) {
        if (this.tourForm.value.shows.length) {
          let tour = automapper.map(
            'TourFormModel',
            'TourWithManagerAndShowsForCreation',
            this.tourForm.value);
          this.tourService.addTourWithManagerAndShows(tour)
            .subscribe(
              () => {
                this.router.navigateByUrl('/tours');
              },
              (validationResult) => { ValidationErrorHandler.handleValidationErrors(this.tourForm, validationResult); });
        }
        else {
          let tour = automapper.map(
            'TourFormModel',
            'TourWithManagerForCreation',
            this.tourForm.value);
          this.tourService.addTourWithManager(tour)
            .subscribe(
              () => {
                this.router.navigateByUrl('/tours');
              },
              (validationResult) => { ValidationErrorHandler.handleValidationErrors(this.tourForm, validationResult); });
        }
      }
      else {
        if (this.tourForm.value.shows.length) {
          let tour = automapper.map(
            'TourFormModel',
            'TourWithShowsForCreation',
            this.tourForm.value);
          this.tourService.addTourWithShows(tour)
            .subscribe(
              () => {
                this.router.navigateByUrl('/tours');
              },
              (validationResult) => { ValidationErrorHandler.handleValidationErrors(this.tourForm, validationResult); });
        }
        else {
          let tour = automapper.map(
            'TourFormModel',
            'TourForCreation',
            this.tourForm.value);
          this.tourService.addTour(tour)
            .subscribe(
              () => {
                this.router.navigateByUrl('/tours');
              },
              (validationResult) => { ValidationErrorHandler.handleValidationErrors(this.tourForm, validationResult); });
        }
      }
    }
  }
}