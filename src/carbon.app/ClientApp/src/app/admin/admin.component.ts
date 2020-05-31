import {HttpClient} from "@angular/common/http";
import { applicationStatusLabel } from "./applicationStatusLabel";

declare var M: any;

import {AfterViewInit, Component, OnInit} from '@angular/core';
import {config} from "../config";
import {AdminApi} from "../services/api/admin-api";
import { NgSelectOption } from "@angular/forms";
import {AppModalGeneral} from "../components/modal/app-modal-general.component";
import {ApplicationApi} from "../services/api/application-api";
import {DateHelper} from "../services/helpers/date-helper";

@Component({
  selector: 'app-admin-component',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements AfterViewInit {

  private adminApi;
  private applicationApi;

  public filterModal;

  constructor(private http: HttpClient) {
    this.adminApi = new AdminApi(http, this.loading);
    this.applicationApi = new ApplicationApi(http, this.loading);
  }

  public loading = false;

  public tabNo = 1;

  public tab(no) { this.tabNo = no; }

  ngAfterViewInit(): void {
    const elem = document.querySelector('.tabs');
    const options = {};
    M.Tabs.init(elem, options);

    this.getApplications();

    this.filterModal = AppModalGeneral.getModalInstance("filterModal");
  }

  public users = {};

  public getUsers() {
    this.loading = true;
    this.adminApi.GetUsers().subscribe(data => {
      this.users = data;
      this.loading = false;
    });
  }

  public applicationPackage = {
    applicationCount: 0,
    applications: [],
    applicationCountries: [],
    applicationStates: []
  };

  public getApplications() {
    this.loading = true;
    this.adminApi.getApplicationsPackage(this.filterOptions).subscribe(data => {
      // @ts-ignore
      this.applicationPackage = data;

      this.applicationPackage.applications.forEach(application => {
        application.statusLabel = applicationStatusLabel.get(application.status);
        AdminComponent.ageCalculation(application);
      });

      const maxPagesFloat = Math.ceil(this.applicationPackage.applicationCount/this.filterOptions.resultsPerPage);

      this.maxPages = parseInt(maxPagesFloat.toString());

      this.getCountries();
    });
  }

  private static ageCalculation(application) {
    let dob = new Date(application.dateOfBirth);
    let mootStart = new Date(2022, 12, 31, 0);
    let applicationAgeAtMoot = DateHelper.daysBetween(dob, mootStart);

    application.ageMonths = applicationAgeAtMoot.months;
    application.ageYears = applicationAgeAtMoot.years;
  }

  public countries = [];

  private getCountries() {
    this.loading = true;
    this.applicationApi.getCountries().subscribe(
      data => {
        // @ts-ignore
        this.countries = Object.values(data);
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

  private getStates() {
    this.loading = true;
    this.applicationApi.getStates().subscribe(
      data => {
        // @ts-ignore
        this.states = Object.values(data);

        this.loading = false;
      },
      error => {
        console.log(error);

        M.toast({html: error.error, classes: "rounded red"});
        this.loading = false;
      }
    );
  }

  public filterOptions = {
    countries: null,
    states: null,
    ageDate: "0001-01-01T00:00:00",
    minimumAge: 0,
    maximumAge: 0,
    resultsPerPage: 50,
    page: 1
  };

  public editFilters() {

    this.applicationPackage.applicationCountries.forEach(country => {
      country.filtered = (this.filterOptions.countries != null && this.filterOptions.countries.indexOf(country.id) > -1);
    });

    this.applicationPackage.applicationStates.forEach(state => {
      state.filtered = (this.filterOptions.states != null && this.filterOptions.states.indexOf(state.id) > -1);
    });

    this.filterModal.open();

  }

  public saveFilters() {

    this.filterOptions.countries = [];

    this.applicationPackage.applicationCountries.forEach(country => {
      if (country.filtered) this.filterOptions.countries.push(country.id);
    });

    if (this.filterOptions.countries.length < 1) this.filterOptions.countries = null;

    this.filterOptions.states = [];

    this.applicationPackage.applicationStates.forEach(state => {
      if (state.filtered) this.filterOptions.states.push(state.id);
    });

    if (this.filterOptions.states.length < 1) this.filterOptions.states = null;

    this.getApplications();

    this.filterModal.close();

  }

  public maxPages = 0;

  public setPage(no) {
    if (no > this.maxPages) return;
    if (no < 1) return;
    this.filterOptions.page = no;
    this.getApplications();
  }

  public createRange(number){
    const items: number[] = [];
    for(var i = 1; i <= number; i++){
      items.push(i);
    }
    return items;
  }

}
