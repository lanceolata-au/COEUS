import { JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import {AfterViewChecked, AfterViewInit, Component, OnInit} from '@angular/core';
import { config } from "./config";
import { HttpClient } from "@angular/common/http";
import {ApplicationApi} from "./services/api/app-api";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewChecked {
  title = 'app';

  public loaded = false;

  constructor(private oauthService: OAuthService, private _http: HttpClient) {

    let _authConfig = authConfig;
    authConfig.issuer = config.issuer;

    this.ConfigureImplicitFlowAuthentication(_authConfig);

    this.appApi = new ApplicationApi(_http);

  }

  private ConfigureImplicitFlowAuthentication(_authConfig) {

    this.oauthService.configure(_authConfig);

    this.oauthService.tokenValidationHandler = new JwksValidationHandler();

    this.oauthService.loadDiscoveryDocument().then(doc => {
      this.oauthService.tryLogin()
        .catch(err => {
          console.error(err);
        })
        .then(() => {
          if(!this.oauthService.hasValidAccessToken()) {
          }
        });
    });
  }

  private appApi;

  public loggedIn = false;

  public profile = {
    userName: null,
    coreUserDto: {
      access: 0,
      picture: null
    }
  };

  public logoff() {
    sessionStorage.setItem("profile","");
    this.oauthService.logOut(false);
  }

  private getProfile() {
    let profileJson = sessionStorage.getItem("profile");

    if (profileJson != null && profileJson != "") {

      this.profile = JSON.parse(profileJson);

      this.appApi.getExternalProfile().subscribe(
        data => {
          if (JSON.stringify(data) == profileJson) {
            this.loggedIn = true;
          } else {
            sessionStorage.setItem("profile","");
          }
        },
        error => {
          sessionStorage.setItem("profile","");
          console.log(error);
        }
      );

    }

  }

  ngOnInit(): void {
    this.getProfile();
  }

  ngAfterViewChecked(): void {
    this.loaded = true;
  }



}

