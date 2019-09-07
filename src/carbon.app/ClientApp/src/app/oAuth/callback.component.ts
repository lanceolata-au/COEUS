import {AfterViewInit, Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {getBaseUrl} from "../../main";
import {OAuthService} from "angular-oauth2-oidc";

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html'
})

export class CallbackComponent implements AfterViewInit  {

  constructor(private oauthService: OAuthService ,private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  }

  ngAfterViewInit(): void {
    this.http.get(getBaseUrl() + "App/ExternalProfile").subscribe(
      data => console.log(data),
      error => console.log(error)
    );
  }
}
