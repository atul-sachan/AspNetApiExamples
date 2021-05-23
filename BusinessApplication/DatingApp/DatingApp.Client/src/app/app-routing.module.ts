import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './guards/auth.guard';
import { NotFoundComponent } from './error/not-found/not-found.component';
import { ServerErrorComponent } from './error/server-error/server-error.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children:[
      {path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},
      {path: 'members/:username', component: MemberDetailComponent},
      {path: 'member/edit', component: MemberEditComponent},
      {path: 'lists', component: ListsComponent},
      {path: 'messages', component: MessagesComponent}
    ]
  },
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: "**", component: HomeComponent, pathMatch: "full"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
