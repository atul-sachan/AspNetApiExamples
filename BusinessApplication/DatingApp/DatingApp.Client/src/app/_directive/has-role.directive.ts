import { AccountService } from './../_services/account.service';
import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs/operators';
import { User } from '../_models/user.model';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  user: User;

  constructor(private vc: ViewContainerRef, private temppleteRef: TemplateRef<any>, private accountService: AccountService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user = user;
    })
  }

  ngOnInit(): void {
    if(!this.user?.roles || this.user == null){
      this.vc.clear();
      return;
    }

    if(this.user?.roles.some(r=> this.appHasRole.includes(r))){
      this.vc.createEmbeddedView(this.temppleteRef);
    }else{
      this.vc.clear();
    }
  }

}
