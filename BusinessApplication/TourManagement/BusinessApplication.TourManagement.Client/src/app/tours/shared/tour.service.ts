import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseService } from "./base.service";
import { Tour } from "./tour.model";
import { TourWithEstimatedProfits } from './tour-with-estimated-profits.model';

@Injectable()
export class TourService extends BaseService {

    constructor(private http: HttpClient) {           
        super();      
    }

    getTours(): Observable<Tour[]> {
        return this.http.get<Tour[]>(`${this.apiUrl}/tours`);
    }

    getTour(tourId: string): Observable<Tour> {
        return this.http.get<Tour>(`${this.apiUrl}/tours/${tourId}`);
    }

    getTourWithEstimatedProfits(tourId: string): Observable<TourWithEstimatedProfits> {
        return this.http.get<TourWithEstimatedProfits>(`${this.apiUrl}/tours/${tourId}`, 
        { headers: { 'Accept': 'application/vnd.marvin.tourwithestimatedprofits+json'}});
    }
}