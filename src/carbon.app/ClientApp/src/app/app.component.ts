import {JwksValidationHandler, OAuthService} from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { AuthConfig } from 'angular-oauth2-oidc';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  constructor(private oauthService: OAuthService) {

    this.configure();

  }

  private configure() {

    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin();

  }
}

