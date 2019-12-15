import { authConfig } from '../auth.config';
import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  loginFailed: boolean = false;
  userProfile: object;

  constructor(private oauthService: OAuthService, private http: HttpClient) {

  }

  ngOnInit() {

  }

}
