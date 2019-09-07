import {JwksValidationHandler, OAuthService} from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  constructor(private oauthService: OAuthService) {
    this.ConfigureImplicitFlowAuthentication()
  }

  private ConfigureImplicitFlowAuthentication() {

    this.oauthService.configure(authConfig);

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

