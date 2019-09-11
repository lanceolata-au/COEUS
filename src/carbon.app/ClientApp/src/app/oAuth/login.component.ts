// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
import { Component, OnInit } from '@angular/core';
import { OAuthService} from "angular-oauth2-oidc";

@Component({
  selector: 'app-profile',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private oauthService: OAuthService) { }

  ngOnInit() {

    if (this.oauthService.getAccessTokenExpiration() > Date.now() || this.oauthService.getAccessTokenExpiration() == null) {

      this.oauthService.getIdentityClaims();

      this.oauthService.initLoginFlow();

    }


  }

}
// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
