import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../models/user.model';
import { ReplaySubject } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl = environment.apiUrl;
  private currentUserStore = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserStore.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User)=>{
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserStore.next(user);
        }
        return user;
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User)=>{
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserStore.next(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: User){
    this.currentUserStore.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserStore.next(null);
  }

}
