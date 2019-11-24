import {AfterViewInit, Component, OnInit} from '@angular/core';
import {getBaseUrl} from "../../main";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-application-preliminary-component',
  templateUrl: './applicationPreliminary.component.html'
})
export class ApplicationPreliminaryComponent implements OnInit {

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
    this.http.get(getBaseUrl() + "Application/GetStatus").subscribe(
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
