import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";

@Injectable()
export class BaseService {
  apiUrl = environment.apiUrl;

  constructor() { }

}