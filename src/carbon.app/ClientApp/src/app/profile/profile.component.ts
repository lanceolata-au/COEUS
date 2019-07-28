import { Component, OnInit } from '@angular/core';
import { OAuthService } from "angular-oauth2-oidc";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(private oauthService: OAuthService) { }

  claims = null;

  public login() {
    this.oauthService.initLoginFlow();
  }

  public logoff() {
    this.oauthService.logOut();
  }

  ngOnInit() {
    let claims = this.oauthService.getIdentityClaims();
  }

}
