import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OAuthService } from "angular-oauth2-oidc";

@Injectable({
  providedIn: 'root'
})

export class AuthedHttpClientService {

  constructor(private oauthService: OAuthService, private http: HttpClient) {

  }

}
