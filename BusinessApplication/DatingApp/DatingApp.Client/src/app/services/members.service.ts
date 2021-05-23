import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Member } from '../models/member.model';
import { Observable } from 'rxjs';

const httpOptions = {
    headers: new HttpHeaders({
        Authorization: 'Bearer '+ JSON.parse(localStorage.getItem('user'))?.token
    })
}

@Injectable({ providedIn: 'root' })
export class MembersService {
    baseUrl = environment.apiUrl;

    constructor(private httpClient: HttpClient) { }

    getMembers(): Observable<Member[]>{
        return this.httpClient.get<Member[]>(this.baseUrl + 'users')
    }

    getMember(username: string): Observable<Member>{
        return this.httpClient.get<Member>(this.baseUrl + 'users/'+username)
    }
}