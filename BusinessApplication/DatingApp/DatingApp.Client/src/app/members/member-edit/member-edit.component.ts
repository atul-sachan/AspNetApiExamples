import { MembersService } from './../../services/members.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/models/member.model';
import { User } from 'src/app/models/user.model';
import { AccountService } from '../../services/account.service';
import { take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm

  member: Member;
  user: User;
  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
   }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    this.memberService.getMember(this.user.userName).subscribe(member => this.member = member);
  }

  updateMember(){
    console.log(this.member);
    this.toastr.success("Form submitted Successfully");
    this.editForm.reset(this.member);
  }

}
