import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { tap, map } from 'rxjs/operators'; 
import { BaseService } from "./base.service";
import { Manager } from "./manager.model";
import { Band } from './band.model';

@Injectable()
export class MasterDataService extends BaseService {

  private bands: Band[];
  private managers: Manager[];

  constructor(private http: HttpClient) {
    super();
  }

  getBands(): Observable<Band[]> {
    if (this.bands) {
      return of(this.bands);
    }
    else {
      return this.http.get<Band[]>(`${this.apiUrl}/bands`).pipe(
          tap((bandsFromResponse) => {
            this.bands = bandsFromResponse;
          })
      )
    }
  }

  getManagers(): Observable<Manager[]> {
    if (this.managers) {
      return of(this.managers);
    }
    else {
      return this.http.get<Manager[]>(`${this.apiUrl}/managers`).pipe(
        tap((managersFromResponse) => {
          this.managers = managersFromResponse;
        })
      )
    }
  }
}