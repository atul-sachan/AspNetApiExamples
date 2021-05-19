import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Band } from '../shared/band.model';
import { MasterDataService } from '../shared/master-data.service';
import { TourService } from '../shared/tour.service';

@Component({
  selector: 'app-tour-add',
  templateUrl: './tour-add.component.html',
  styleUrls: ['./tour-add.component.scss']
})
export class TourAddComponent implements OnInit {

  public tourForm: FormGroup;
  bands: Band[];

  constructor(private masterDataService: MasterDataService,
    private tourService: TourService,
    private formBuilder: FormBuilder,
    private router: Router) { }

  ngOnInit() {

    // define the tourForm (with empty default values)
    this.tourForm = this.formBuilder.group({
      band: [''],
      title: [''],
      description: [''],
      startDate: [],
      endDate: []
    });

    // get bands from master data service
    this.masterDataService.getBands()
      .subscribe(bands => {
        this.bands = bands;
      });    
  }

  addTour(): void {
    if (this.tourForm.dirty) {
        // TODO
    }
  }

}