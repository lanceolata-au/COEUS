import { Component } from '@angular/core';
import * as M from 'materialize-css';
import {AfterViewInit} from "@angular/core/src/metadata/lifecycle_hooks";
import {OAuthService} from "angular-oauth2-oidc";
import {HttpClient} from "@angular/common/http";
import {config} from "../config";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})

export class NavMenuComponent implements AfterViewInit {
  isExpanded = false;

  constructor(private oauthService: OAuthService, private http: HttpClient) {

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

  ngAfterViewInit(): void {

    const sidenav = document.querySelectorAll('.sidenav');
    M.Sidenav.init(sidenav);

    document.addEventListener('DOMContentLoaded', function() {
      var elems = document.querySelectorAll('.dropdown-trigger');
      var instances = M.Dropdown.init(elems, {});
    });

    this.getProfile();

  }

  private getProfile() {
    let profileJson = sessionStorage.getItem("profile");

    if (profileJson != null) {

      this.profile = JSON.parse(profileJson);

      this.http.get( config.baseUrl + "App/ExternalProfile").subscribe(
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
