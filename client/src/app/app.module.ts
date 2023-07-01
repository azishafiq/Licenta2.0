import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './mebmer/member-list/member-list.component';
import { MemberDetailComponent } from './mebmer/member-detail/member-detail.component';
import { MemberCardComponent } from './mebmer/member-card/member-card.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';


import { SharedModule } from './_modules/shared.module';
import { ErrorIterceptor } from './_interceptors/error.interceptor';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { MemberEditComponent } from './mebmer/member-edit/member-edit.component';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { PhotoEditorComponent } from './mebmer/photo-editor/photo-editor.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';

import {MatDatepickerModule} from '@angular/material/datepicker';
import { MemberMessagesComponent } from './mebmer/member-messages/member-messages.component';
import { ProjectEditorComponent } from './mebmer/project-editor/project-editor.component';



@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorComponent,
    ServerErrorComponent,
    NotFoundComponent,
    TextInputComponent,
    MemberMessagesComponent,
    ProjectEditorComponent  
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    SharedModule,
    BrowserAnimationsModule,
    ReactiveFormsModule
    
    
    

    
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorIterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
