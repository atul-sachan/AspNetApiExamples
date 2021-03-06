import { AccountService } from './../../_services/account.service';
import { PresenceService } from './../../_services/presence.service';
import { MessageService } from './../../_services/message.service';
import { Message } from './../../_models/message.model';
import { Member } from '../../_models/member.model';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { User } from '../../_models/user.model';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true }) memberTabs: TabsetComponent ;
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab: TabDirective;
  messages: Message[] = [];
  user: User;
  constructor(
    private memberService: MembersService, 
    private route: ActivatedRoute, 
    private messageService: MessageService, 
    public presenceService: PresenceService,
    private accountService: AccountService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.member = data.member;
    })

    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })
    
    this.galleryOptions = [{
      width: '700px',
      height: '500px',
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }];

    this.galleryImages = this.getImages();
  }

  getImages(): NgxGalleryImage[]{
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      })
    }

    return imageUrls;
  }

  loadMessages() {
    this.messageService.getMessageThread(this.member.userName).subscribe(messages => {
      this.messages = messages;
      console.log(messages);
    })
  }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.messageService.createHubConnection(this.user, this.member.userName )
    }else{
      this.messageService.stopHubConnection();
    }
  }

  ngOnDestroy(){
    this.messageService.stopHubConnection();
  }

}
