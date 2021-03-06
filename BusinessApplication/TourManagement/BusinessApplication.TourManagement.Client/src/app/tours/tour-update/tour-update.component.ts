import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { compare } from 'fast-json-patch';
import { Subscription } from 'rxjs';
import { CustomValidators } from '../shared/custom-validators';
import { MasterDataService } from '../shared/master-data.service';
import { TourForUpdate } from '../shared/tour-for-update.model';
import { Tour } from '../shared/tour.model';
import { TourService } from '../shared/tour.service';

@Component({
  selector: 'app-tour-update',
  templateUrl: './tour-update.component.html',
  styleUrls: ['./tour-update.component.scss']
})
export class TourUpdateComponent implements OnInit, OnDestroy {
  public tourForm: FormGroup;
  tour: Tour;
  private tourId: string;
  private sub: Subscription;
  private originalTourForUpdate: TourForUpdate;

  constructor(private masterDataService: MasterDataService,
    private tourService: TourService,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder) { }


  ngOnInit(): void {
    // define the tourForm (with empty default values)
    this.tourForm = this.formBuilder.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      startDate: [, Validators.required],
      endDate: [, Validators.required]
    }, { validator: CustomValidators.StartDateBeforeEndDateValidator });

    // get route data (tourId)
    this.sub = this.route.params.subscribe(
      params => {
        this.tourId = params['tourId'];

        // load tour
        this.tourService.getTour(this.tourId)
          .subscribe(tour => {
            this.tour = tour;
            this.updateTourForm();

            this.originalTourForUpdate = automapper.map(
              'TourFormModel',
              'TourForUpdate',
              this.tourForm.value);

          });
      }
    );
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  private updateTourForm(): void {
    let datePipe = new DatePipe(navigator.language);
    let dateFormat = 'yyyy-MM-dd';

    this.tourForm.patchValue({
      title: this.tour.title,
      description: this.tour.description,
      startDate: datePipe.transform(this.tour.startDate, dateFormat),
      endDate: datePipe.transform(this.tour.endDate, dateFormat),
    });
  }

  saveTour(): void {
    if (this.tourForm.dirty && this.tourForm.valid) {
      // TODO
      // [
      //   { op:??"replace",??path:??"/description",??value:??"Updated description"}
      //   {op:??"replace",??path:??"/title",??value:??"Updated title"}
      // ]

      let changedTourForUpdate = automapper.map(
        'TourFormModel',
        'TourForUpdate',
        this.tourForm.value);

      let patchDocument = compare(this.originalTourForUpdate, changedTourForUpdate);

      this.tourService.partiallyUpdateTour(this.tourId, patchDocument)
        .subscribe(
          () => {
            this.router.navigateByUrl('/tours');
          });
    }
  }
}
