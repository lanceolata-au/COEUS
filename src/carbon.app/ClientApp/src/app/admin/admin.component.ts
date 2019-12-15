import {HttpClient} from "@angular/common/http";
import { applicationStatusLabel } from "./applicationStatusLabel";

declare var M: any;
import {Component, OnInit} from '@angular/core';
import {config} from "../config";

@Component({
  selector: 'app-admin-component',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  constructor(private http: HttpClient) {

  }

  public loading = false;

  public tabNo = 1;

  public tab1() { this.tabNo = 1; }

  public tab2() { this.tabNo = 2; }

  public tab3() { this.tabNo = 3; }

  public tab4() { this.tabNo = 4; }

  ngOnInit(): void {
    this.getUsers();
    const elem = document.querySelector('.tabs');
    const options= {};
    M.Tabs.init(elem, options);

    const elems_collapsible = document.querySelectorAll('.collapsible');
    M.Collapsible.init(elems_collapsible, {});

    this.getApplications();
  }

  public users = {};

  public getUsers() {
    this.loading = true;
    this.http.get(config.baseUrl + "Admin/GetUsers").subscribe(data => {
      this.users = data;
      this.loading = false;
    });
  }

  public applications = [];
  public countryApplications = [];

  public getApplications() {
    this.http.get(config.baseUrl + "Admin/GetApplications").subscribe(data => {
      this.applications = Object.values(data);
      this.applications.forEach(application => {
        application.statusLabel = applicationStatusLabel.get(application.status);
      });
      this.getCountries();
    });
  }

  public countries = [];
  public countriesApplied = [];

  private getCountries() {
    this.loading = true;
    this.http.get(config.baseUrl + "Application/GetCountries").subscribe(
      data => {
        // @ts-ignore
        this.countries = Object.values(data);

        this.countriesApplied = [];
        this.countryApplications = [];

        this.countries.forEach(country => {
          let countryApplications = [];

          this.applications.forEach(application => {
            if (application.country === country.id) {
              countryApplications = countryApplications.concat([application]);
            }
          });

          if (countryApplications.length > 0) {
            this.countriesApplied = this.countriesApplied.concat([country]);
            this.countryApplications = this.countryApplications.concat([countryApplications]);
          } else {
            this.countryApplications = this.countryApplications.concat([null]);
          }
        });

        this.getStates();

      },
      error => {
        console.log(error);

        M.toast({html: error.error, classes: "rounded red"});
        this.loading = false;
      }
    );
  }

  public states = [];
  public statesApplied = [];
  public stateApplications = [];

  private getStates() {
    this.loading = true;
    this.http.get(config.baseUrl + "Application/GetStates").subscribe(
      data => {
        // @ts-ignore
        const states = Object.values(data);

        this.stateApplications = [];
        this.countries.forEach(country => {

          let stateList = [];
          let countryApplicationGroups = [];

          states.forEach(state => {

            if (state.countryId === country.id) {
              stateList = stateList.concat([state]);

              let stateApplications = [];

              this.applications.forEach(application => {

                if (application.state === state.id) {
                  stateApplications = stateApplications.concat([application]);
                }

              });

              countryApplicationGroups = countryApplicationGroups.concat([stateApplications]);

            } else {

              stateList = stateList.concat([null]);
              countryApplicationGroups = countryApplicationGroups.concat([[]]);

            }
          });

          this.states = this.states.concat([stateList]);
          this.stateApplications = this.stateApplications.concat([countryApplicationGroups]);

        });

        this.statesApplied = [];

        this.countries.forEach(country => {
          let stateList = [];

          states.forEach(state => {

            if (state.countryId === country.id) {
              stateList = stateList.concat([state]);
            }

          });

          if (stateList.length > 1) {
            this.statesApplied = this.statesApplied.concat([stateList]);
          } else {
            this.statesApplied = this.statesApplied.concat([null]);
          }

        });

        const elems_collapsible = document.querySelectorAll('.collapsible');
        M.Collapsible.init(elems_collapsible);
        this.loading = false;
      },
      error => {
        console.log(error);

        M.toast({html: error.error, classes: "rounded red"});
        this.loading = false;
      }
    );
  }

}
