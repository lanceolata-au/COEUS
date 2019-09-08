import { Component } from '@angular/core';
import * as M from 'materialize-css';
import {AfterViewInit} from "@angular/core/src/metadata/lifecycle_hooks";
import {OAuthService} from "angular-oauth2-oidc";
import {LoginEmitterService} from "../services/login-emitter.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})

export class NavMenuComponent implements AfterViewInit {
  isExpanded = false;

  constructor(private oauthService: OAuthService) {

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

  private loggedIn = false;
  private profile = {userName: null};

  ngAfterViewInit(): void {

    const sidenav = document.querySelectorAll('.sidenav');
    M.Sidenav.init(sidenav);

    this.getProfile();

    //this.emitterService.subscribeToLogin(() => this.getProfile());

  }

  private getProfile() {
    let profileJson = sessionStorage.getItem("profile");

    if (profileJson != null) {

      this.profile = JSON.parse(profileJson);

      this.loggedIn = true;
    }

  }

}
