import {AfterViewChecked, Component, OnInit} from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import {HttpClient} from "@angular/common/http";
import {ApplicationApi} from "../services/api/app-api";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, AfterViewChecked {

  private appApi;

  constructor(private oauthService: OAuthService, private http: HttpClient) {
    this.appApi = new ApplicationApi(http);
  }

  public loaded = false;

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
          this.logoff();
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
