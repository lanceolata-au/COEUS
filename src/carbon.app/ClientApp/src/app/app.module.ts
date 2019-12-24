import { APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { config} from "./config";
import { environment } from "../environments/environment";
import { AuthedHttpClientService } from "./services/authed-http-client.service";

// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
import {OAuthModule, OAuthService} from 'angular-oauth2-oidc';
import { CallbackComponent } from "./oAuth/callback.component";
import { LoginComponent } from "./oAuth/login.component";
// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';

import { ProfileComponent } from './profile/profile.component';

import { AdminComponent } from "./admin/admin.component";
import { PrivacyComponent } from "./info-pages/privacy.component";

import { ApplicationComponent } from "./application/application.component";
import { ApplicationPreliminaryComponent } from "./application/application-preliminary.component";
import { ApplicationPreliminaryBulkComponent } from "./application/application-preliminary-bulk.component";

import { AppLoaderService } from "./components/loading/app-loader-service.component";
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { NavFooterComponent } from './components/nav-footer/nav-footer.component';

export function load(http: HttpClient): (() => Promise<boolean>) {
  return (): Promise<boolean> => {
    return new Promise<boolean>((resolve: (a: boolean) => void): void => {
      http.get('./assets/config/config.' + environment.name + '.conf')
        .subscribe( data => {
            // @ts-ignore
            config.issuer = data.issuer;
            // @ts-ignore
            config.version = data.version;
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
    ApplicationComponent,
    ApplicationPreliminaryComponent,
    ApplicationPreliminaryBulkComponent,
    ProfileComponent,
    PrivacyComponent,

    // App wide components
    AppLoaderService

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
      { path: 'login', component: LoginComponent },
      { path: 'profile', component: ProfileComponent },
      { path: 'admin', component: AdminComponent },
      { path: 'callback', component: CallbackComponent },
      { path: 'privacy', component: PrivacyComponent },
      //{ path: 'application', component: ApplicationComponent},
      { path: 'application-preliminary', component: ApplicationPreliminaryComponent},
      { path: 'application-preliminary-bulk', component: ApplicationPreliminaryBulkComponent}
    ])
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: load,
      multi: true,
      deps: [
        HttpClient
      ]
    },
    {
      // =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
      provide: HTTP_INTERCEPTORS,
      useClass: AuthedHttpClientService,
      multi: true,
      deps: [
        OAuthService
      ]
      // =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
