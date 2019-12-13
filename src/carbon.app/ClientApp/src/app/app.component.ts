import {JwksValidationHandler, OAuthService} from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { Component } from '@angular/core';
import {config} from "./config";
import {environment} from "../environments/environment";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  constructor(private oauthService: OAuthService, private _http: HttpClient) {

    let _authConfig = authConfig;
    authConfig.issuer = config.issuer;

    this.ConfigureImplicitFlowAuthentication(_authConfig)

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

}

