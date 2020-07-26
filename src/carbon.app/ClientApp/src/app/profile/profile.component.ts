import {AfterViewChecked, Component, OnInit} from '@angular/core';
import {ProfileApi} from "../services/api/profile-api";
import {HttpClient} from "@angular/common/http";
import {DateHelper} from "../services/helpers/date-helper";
import {ApplicationApi} from "../services/api/application-api";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, AfterViewChecked {

  public loading = false;
  public readyToRender = false;

  private applicationApi: ApplicationApi;

  private profileApi;

  constructor(private http: HttpClient) {
    this.profileApi = new ProfileApi(http, false);
    this.applicationApi = new ApplicationApi(http, false);
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

  public countries = [];

  private getCountries() {
    this.loading = true;
    this.applicationApi.getCountries().subscribe(
      data => {
        // @ts-ignore
        this.countries = Object.values(data);
        //M.FormSelect.init(this.elems_select);
        this.getStates();
      },
      error => {
        console.log(error);

        //M.toast({html: error.error, classes: "rounded red"});
      }
    );
  }

  public states = [];

  private getStates() {
    this.loading = true;
    this.applicationApi.getStates().subscribe(
      data => {
        // @ts-ignore
        const states = Object.values(data);

        this.countries.forEach(county => {
          var stateList = [];
          states.forEach(state => {
            if (state.countryId === county.id) {
              stateList = stateList.concat([state]);
            }
          });

          if (stateList.length < 1) stateList = stateList.concat(
            [{
              countryId: county.id,
              shortCode: "NN",
              fullName: "N/A",
              id: 0
            }]);

          this.states = this.states.concat([stateList]);
          this.loading = false;
        });
      },
      error => {
        console.log(error);

        //M.toast({html: error.error, classes: "rounded red"});
        this.loading = false;
      }
    );
  }

  ngOnInit() {
    this.getCountries();
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

  public countryChange() {
    let state = this.states[this.fullProfile.country - 1][0];
    this.fullProfile.state = state.id;
  }

  private elems_select;

  ngAfterViewChecked(): void {
    //M.updateTextFields();

    const elems_select = document.querySelectorAll('select');
    this.elems_select = elems_select;
    //M.FormSelect.init(elems_select);
  }

}
