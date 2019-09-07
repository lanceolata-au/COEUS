import { authConfig } from '../auth.config';
import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { getBaseUrl } from "../../main";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  loginFailed: boolean = false;
  userProfile: object;

  constructor(private oauthService: OAuthService, private http: HttpClient) {

  }

  ngOnInit() {
    this.http.get(getBaseUrl() + "App/ExternalProfile").subscribe(
      data => console.log(data),
      error => console.log(error)
    );
  }

  get requestAccessToken() {
    return this.oauthService.requestAccessToken;
  }


}
