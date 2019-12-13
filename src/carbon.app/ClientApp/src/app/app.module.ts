import { APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { NavFooterComponent } from './nav-footer/nav-footer.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { ApplicationComponent} from "./application/application.component";
import { ApplicationPreliminaryComponent} from "./application/application-preliminary.component";

// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
import { OAuthModule } from 'angular-oauth2-oidc';
import { CallbackComponent } from "./oAuth/callback.component";
import { LoginComponent } from "./oAuth/login.component";
// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=

import { ProfileComponent } from './profile/profile.component';
import {AuthedHttpClientService} from "./services/authed-http-client.service";
import {AdminComponent} from "./admin/admin.component";
import {PrivacyComponent} from "./info-pages/privacy.component";
import {environment} from "../environments/environment";
import {config} from "./config";
import {authConfig} from "./auth.config";
import {catchError} from "rxjs/operators";
import {Observable, ObservableInput} from "rxjs";

function load(http: HttpClient): (() => Promise<boolean>) {
  return (): Promise<boolean> => {
    return new Promise<boolean>((resolve: (a: boolean) => void): void => {
      http.get('./assets/config/config.dev.conf')
        .subscribe( data => {
            // @ts-ignore
            config.issuer = data.issuer;
            config.baseUrl = config.issuer + "/";
            resolve(true);
        },
          error => {
          if (error.status !== 404) {
            resolve(false);
          }
          config.baseUrl = 'http://localhost:8080/api';
          resolve(true);
        });
    });
  };
}

@NgModule({
  declarations: [
    AppComponent,

    // =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
    LoginComponent,
    CallbackComponent,
    // =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=

    NavMenuComponent,
    NavFooterComponent,
    HomeComponent,
    AdminComponent,
    CounterComponent,
    ApplicationComponent,
    ApplicationPreliminaryComponent,
    ProfileComponent,
    PrivacyComponent

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
      { path: 'login', component: LoginComponent },
      { path: 'profile', component: ProfileComponent },
      { path: 'admin', component: AdminComponent },
      { path: 'callback', component: CallbackComponent },
      { path: 'privacy', component: PrivacyComponent },
      { path: 'application', component: ApplicationComponent},
      { path: 'application-preliminary', component: ApplicationPreliminaryComponent}
    ])
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: load,
      multi: true,
      deps: [HttpClient]
    },
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
