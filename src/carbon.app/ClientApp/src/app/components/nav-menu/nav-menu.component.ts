import {AfterViewChecked, AfterViewInit, Component, OnInit} from '@angular/core';
import { OAuthService } from "angular-oauth2-oidc";
import { HttpClient } from "@angular/common/http";
import { ApplicationApi } from "../../services/api/app-api";

import  * as M from "../../../assets/materializescss/js/compiled/materialize.js";
import {ProfileApi} from "../../services/api/profile-api";
import {AppComponent} from "../../app.component";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})

export class NavMenuComponent implements OnInit, AfterViewInit, AfterViewChecked {

  public isExpanded = false;
  public loaded = false;

  private appApi;
  private profileApi;

  constructor(private oauthService: OAuthService, private http: HttpClient) {
    this.appApi = new ApplicationApi(http);
    this.profileApi = new ProfileApi(http);
  }

  public logoff() {
    sessionStorage.setItem("profile","");
    this.oauthService.logOut(false);
  }

  collapse() {
    this.isExpanded = false;
    const sidenav = document.querySelectorAll('.sidenav');
    const instance = M.Sidenav.getInstance(sidenav[0]);
    instance.close();
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
    const sidenav = document.querySelectorAll('#loggedInInfo');
    const instance = M.Sidenav.getInstance(sidenav[0]);
    if (this.isExpanded) {
      instance.open();
    } else {
     instance.close();
    }

  }

  public loggedIn = false;

  public profile = {
    userName: null,
    coreUserDto: {
      access: 0,
      picture: null
    }
  };

  private loginDropDown;

  private profileReturn = {
    loggedIn: false,
    profile: {
      userName: null,
      coreUserDto: {
        access: 0,
        picture: null
      }
    }
  };

  ngOnInit(): void {

    this.getProfile();

  }

  ngAfterViewInit(): void {

    const sidenav = document.querySelectorAll('.sidenav');
    M.Sidenav.init(sidenav);

    let dropdown = document.querySelectorAll('.dropdown-trigger');
    this.loginDropDown = M.Dropdown.init(dropdown, {});

  }

  ngAfterViewChecked(): void {
    this.loaded = true;
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

}
