import { environment } from '../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Member } from '../_models/member.model';
import { Observable, of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { PaginatedResult } from '../_models/pagination.model';
import { User } from '../_models/user.model';
import { UserParams } from '../_models/user-params.model';
import { AccountService } from './account.service';

const httpOptions = {
    headers: new HttpHeaders({
        Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
    })
}

@Injectable({ providedIn: 'root' })
export class MembersService {
    baseUrl = environment.apiUrl;
    members: Member[] = [];
    memberCache = new Map();
    user: User;
    userParams: UserParams;

    constructor(private httpClient: HttpClient, private accountService: AccountService) {
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
            this.user = user;
            this.userParams = new UserParams(user);
        })
    }

    getUserParams() {
        return this.userParams;
    }

    setUserParams(params: UserParams) {
        this.userParams = params;
    }

    resetUserParams() {
        this.userParams = new UserParams(this.user);
        return this.userParams;
    }


    getMembers(userParams: UserParams) {
        var response = this.memberCache.get(Object.values(userParams).join('-'));
        if (response) {
            return of(response);
        }

        let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

        params = params.append('minAge', userParams.minAge.toString());
        params = params.append('maxAge', userParams.maxAge.toString());
        params = params.append('gender', userParams.gender);
        params = params.append('orderBy', userParams.orderBy);

        return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
            .pipe(map(response => {
                this.memberCache.set(Object.values(userParams).join('-'), response);
                return response;
            }))
    }

    // getMembers(): Observable<Member[]>{
    //     if(this.members.length > 0) return of(this.members);
    //     return this.httpClient.get<Member[]>(this.baseUrl + 'users').pipe(
    //         map(members => {
    //             this.members = members;
    //             return members;
    //         })
    //     )
    // }

    getMember(username: string): Observable<Member> {
        const member = [...this.memberCache.values()]
            .reduce((arr, elem) => arr.concat(elem.result), [])
            .find((member: Member) => member.userName === username);

        console.log(member);
        if (member) {
            return of(member);
        }
        return this.httpClient.get<Member>(this.baseUrl + 'users/' + username);
    }

    updateMember(member: Member) {
        return this.httpClient.put(this.baseUrl + 'users', member).pipe(
            map(() => {
                const index = this.members.indexOf(member);
                this.members[index] = member;
            })
        );
    }

    setMainPhoto(photoId: number) {
        return this.httpClient.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
    }

    deletePhoto(photoId: number) {
        return this.httpClient.delete(this.baseUrl + 'users/delete-photo/' + photoId);
    }

    private getPaginatedResult<T>(url, params) {
        const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
        return this.httpClient.get<T>(url, { observe: 'response', params }).pipe(
            map(response => {
                paginatedResult.result = response.body;
                if (response.headers.get('Pagination') !== null) {
                    paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
                }
                return paginatedResult;
            })
        );
    }

    private getPaginationHeaders(pageNumber: number, pageSize: number) {
        let params = new HttpParams();

        params = params.append("pageNumber", pageNumber.toString());
        params = params.append("pageSize", pageSize.toString());

        return params;
    }
}