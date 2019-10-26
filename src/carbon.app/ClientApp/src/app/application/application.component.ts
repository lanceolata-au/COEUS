import {AfterViewInit, Component} from '@angular/core';
import {getBaseUrl} from "../../main";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-application-component',
  templateUrl: './application.component.html'
})
export class ApplicationComponent implements AfterViewInit {

  constructor(private http: HttpClient) {
  }

  private profile = {
    userName: null,
    coreUserDto: {
      id: null,
      access: 0,
      picture: null
    }
  };

  private status;

  ngAfterViewInit(): void {
  }

  private getApplicationStatus() {
    this.http.get(getBaseUrl() + "Application/" + this.profile.coreUserDto.id + "/status").subscribe(
      data => {
        this.status = data
      },
      error => console.log(error)
    );
  }

  private getProfile() {
    let profileJson = sessionStorage.getItem("profile");

    if (profileJson != null) {

      this.profile = JSON.parse(profileJson);
    }
  }

}
