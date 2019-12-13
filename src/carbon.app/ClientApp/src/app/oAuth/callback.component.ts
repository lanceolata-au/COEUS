// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
import {AfterViewInit, Component} from '@angular/core';
import {Router} from "@angular/router"
import {OAuthService} from "angular-oauth2-oidc";
import {HttpClient} from "@angular/common/http";
import {config} from "../config";

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.css']
})

export class CallbackComponent implements AfterViewInit  {

  constructor(private router: Router, private oauthService: OAuthService, private http: HttpClient) {
  }

  ngAfterViewInit(): void {
    this.checkAuthToken();
  }


  private checkAuthToken() {
    if (this.oauthService.getAccessToken() == null) {

      setTimeout(() => {
        if (this.oauthService.getAccessToken() == null) {

          setTimeout(() => this.checkAuthToken(),100);

        }
        else {

          this.http.get(config.baseUrl + "App/ExternalProfile").subscribe(
            data => {
              sessionStorage.setItem("profile", JSON.stringify(data));
              this.router.navigate(['/']).then(() => location.reload());
            },
            error => console.log(error)
          );
        }
      },100);

    }
    else {

      this.http.get(config.baseUrl + "App/ExternalProfile").subscribe(
        data => {
          sessionStorage.setItem("profile", JSON.stringify(data));
          this.router.navigate(['/']).then(() => location.reload());
        },
        error => console.log(error)
      );
    }
  }

}
// =-= BEWARE HERE LIE DRAGONS, AUTH CONFIG IS COMPLETED HERE =-=
