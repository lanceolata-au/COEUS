import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { NavFooterComponent } from './nav-footer/nav-footer.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { CallbackComponent } from "./oAuth/callback.component";
// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
import { OAuthModule } from 'angular-oauth2-oidc';
// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
import { ProfileComponent } from './profile/profile.component';
import {AuthedHttpClientService} from "./services/authed-http-client.service";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    NavFooterComponent,
    HomeComponent,
    CounterComponent,
    ProfileComponent,
    CallbackComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    // =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
    OAuthModule.forRoot(),
    // =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'profile', component: ProfileComponent },
      { path: 'callback', component: CallbackComponent }
    ])
  ],
  providers: [
    {
      // =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
      provide: HTTP_INTERCEPTORS,
      useClass: AuthedHttpClientService,
      multi: true
      // =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
