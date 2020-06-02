import {AfterViewChecked, Component, OnInit} from '@angular/core';
import * as M from 'materialize-css';
import {ProfileApi} from "../services/api/profile-api";
import {HttpClient} from "@angular/common/http";
import {DateHelper} from "../services/helpers/date-helper";
import {CountryStateHelper} from "../services/helpers/country-state-helper";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, AfterViewChecked {

  public loading = false;
  public readyToRender = false;

  private profileApi;
  private countryStateHelper;

  public countries;
  public states;

  constructor(private http: HttpClient) {
    this.countryStateHelper = new CountryStateHelper(http);
    this.profileApi = new ProfileApi(http, false);
  }

  public profile = {
    userName: null,
    coreUserDto: {
      access: 0,
      picture: null
    }
  };

  public fullProfile = {
    applicationMedical: null,
    country: null,
    dateOfBirth: null,
    formation: null,
    id: null,
    name: null,
    phoneNo: null,
    registrationNo: null,
    state: null,
    status: null,
    userId: null,
    applicationAgeAtMoot: {
      years: 0,
      months: 0
    }
  };

  ngOnInit() {
    this.countryStateHelper.getFormatted();

    this.getProfile();
  }

  private getProfile() {
    let profileJson = sessionStorage.getItem("profile");

    if (profileJson != null) {

      this.profile = JSON.parse(profileJson);

    }

    this.profileApi.getMyFull().subscribe(data => {
      this.fullProfile = data;
      DateHelper.dateReformat(this.fullProfile);
      this.readyToRender = true;
    })

  }

  ngAfterViewChecked(): void {
    M.updateTextFields();
  }

}
