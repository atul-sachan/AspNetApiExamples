import { Component, OnInit } from '@angular/core';
import { Tour } from './shared/tour.model';
import { TourService } from './shared/tour.service';

@Component({
  selector: 'app-tours',
  templateUrl: './tours.component.html',
  styleUrls: ['./tours.component.scss']
})
export class ToursComponent implements OnInit {
  title: string = 'Tour overview'
  tours: Tour[] = [];
  
  constructor(private tourService: TourService) { }

  ngOnInit(): void {
    this.tourService.getTours()
      .subscribe(tours => {
          this.tours = tours;
      }); 
  }

}
