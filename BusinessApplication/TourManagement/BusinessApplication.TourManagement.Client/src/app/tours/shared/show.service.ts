import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseService } from "./base.service";
import { Show } from "./show.model";

@Injectable()
export class ShowService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  getShows(tourId: string): Observable<Show[]> {
    return this.http.get<Show[]>(`${this.apiUrl}/tours/${tourId}/shows`);
  }
}