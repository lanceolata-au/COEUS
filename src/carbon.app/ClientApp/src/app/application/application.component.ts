import {AfterViewInit, Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {config} from "../config";

@Component({
  selector: 'app-application-component',
  templateUrl: './application.component.html'
})
export class ApplicationComponent implements OnInit {

  ngOnInit(): void {
     this.getApplicationStatus();
    }
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

  private getApplicationStatus() {
    this.http.get(config.baseUrl + "Application/GetStatus").subscribe(
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
