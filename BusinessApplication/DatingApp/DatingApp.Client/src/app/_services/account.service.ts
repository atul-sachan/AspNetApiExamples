import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../_models/user.model';
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
      map((user: User)=>{
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User)=>{
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: User){
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserStore.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserStore.next(null);
  }

}