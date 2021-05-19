import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Show } from '../shared/show.model';
import { ShowService } from '../shared/show.service';

@Component({
  selector: 'app-shows',
  templateUrl: './shows.component.html',
  styleUrls: ['./shows.component.scss']
})
export class ShowsComponent implements OnInit, OnDestroy{
 
  private sub: Subscription;
  private tourId: string;
  shows: Show[];

  constructor(private showService: ShowService,
     private route: ActivatedRoute) { }

 ngOnInit() {     
      // get route data (tourId)
      this.sub = this.route.params.subscribe(
        params => {
          this.tourId = params['tourId'];
  
          // load tour
          this.showService.getShows(this.tourId)
            .subscribe(shows => {
              this.shows = shows;  
            });
        }
      );
    }

    ngOnDestroy(): void {
      this.sub.unsubscribe();
    }
}