import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseService } from "./base.service";
import { ShowForCreation } from "./show-for-creation.model";
import { Show } from "./show.model";

@Injectable()
export class ShowService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getShows(tourId: string): Observable<Show[]> {
    return this.http.get<Show[]>(`${this.apiUrl}/tours/${tourId}/shows`);
  }

  addShowCollection(tourId: string, showsToAdd: ShowForCreation[]): Observable<Show[]> {
    return this.http.post<Show[]>(`${this.apiUrl}/tours/${tourId}/showcollections`, showsToAdd,
      { headers: { 'Content-Type': 'application/vnd.marvin.showcollectionforcreation+json' } });
  }

}