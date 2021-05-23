import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../services/account.service';
import { User } from '../models/user.model';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

    constructor(private accountService: AccountService){

    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let currenUser: User;
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
            currenUser = user;
        });
        if(currenUser){
            request = request.clone({
                setHeaders:{
                    Authorization: `Bearer ${currenUser.token}`
                }
            }) 
        }
        return next.handle(request);
    }
}   