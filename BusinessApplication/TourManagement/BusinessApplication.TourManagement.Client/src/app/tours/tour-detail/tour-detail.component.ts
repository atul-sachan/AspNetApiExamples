import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { MasterDataService } from '../shared/master-data.service';
import { TourService } from '../shared/tour.service';

@Component({
  selector: 'app-tour-detail',
  templateUrl: './tour-detail.component.html',
  styleUrls: ['./tour-detail.component.scss']
})
export class TourDetailComponent implements OnInit, OnDestroy {

  tour: any;
  private tourId: string;
  private sub: Subscription;
  private isAdmin: boolean = true;

  constructor(private masterDataService: MasterDataService,
    private tourService: TourService,
    private route: ActivatedRoute) {
  }

  ngOnInit() {
    // get route data (tourId)
    this.sub = this.route.params.subscribe(
      params => {
        this.tourId = params['tourId'];

        if (this.isAdmin === true) {
          // get tour with estimated profits field 
          this.tourService.getTourWithEstimatedProfitsAndShows(this.tourId)
            .subscribe(tour => {
              this.tour = tour;
            });
        }
        else {
          // get tour 
          this.tourService.getTourWithShows(this.tourId)
            .subscribe(tour => {
              this.tour = tour;
            });
        }    
      }
    );
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
