import { Component } from '@angular/core';
import { OAuthService } from "angular-oauth2-oidc";

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html'
})

export class CallbackComponent {

  constructor(private oauthService: OAuthService) {
    console.log("token:" + oauthService.getAccessToken());
  }
}
