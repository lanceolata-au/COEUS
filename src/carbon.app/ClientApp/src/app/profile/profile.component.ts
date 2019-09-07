// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
import { Component, OnInit } from '@angular/core';
import { OAuthService} from "angular-oauth2-oidc";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(private oauthService: OAuthService) { }

  public login() {
    this.oauthService.initLoginFlow();
  }

  ngOnInit() {

    if (this.oauthService.getAccessTokenExpiration() > Date.now() || this.oauthService.getAccessTokenExpiration() == null) {

      this.oauthService.getIdentityClaims();

      this.login();

    }


  }

}
// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
