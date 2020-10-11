import { HttpClient } from "@angular/common/http";
import { applicationStatusLabel } from "./applicationStatusLabel";

import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { AdminApi } from "../services/api/admin-api";
import { AppModalGeneral } from "../components/modal/app-modal-general.component";
import { ApplicationApi } from "../services/api/application-api";
import { DateHelper } from "../services/helpers/date-helper";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource}  from "@angular/material/table";

declare var M: any;

export interface application {
  id: number;
  userId: string;
  name: string;
  dateOfBirth: string;
  country: number;
  state: number;
  registrationNo: number;
  formation: number;
  status: number;
  statusLabel: string;
  ageYears: string;
  ageMonths: string;
}

export interface country {
  id: number;
  fullname: string;
  shortCode: string;
  filtered: boolean;
}

export interface state {
  id: number;
  countryId: number;
  fullname: string;
  shortCode: string;
  filtered: boolean;
}

export interface applicationPackage {
  applicationCount: 0,
  applications: application[],
  applicationCountries: country[],
  applicationStates: []
}

@Component({
  selector: 'app-admin-component',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})

export class AdminComponent implements OnInit, AfterViewInit {

  private adminApi;
  private applicationApi;

  public filterModal;

  constructor(private http: HttpClient) {
    this.adminApi = new AdminApi(http);
    this.applicationApi = new ApplicationApi(http);
  }

  public loading = false;

  public tabNo = 1;

  public tab(no) { this.tabNo = no; }

  ngOnInit() {
    this.dataSource.sort = this.sort;
  }

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

  public applicationPackage: applicationPackage = new class implements applicationPackage {
    applicationCount: 0;
    applicationCountries: country[];
    applicationStates: [];
    applications: application[];
  };

  @ViewChild(MatSort, {static: false}) sort: MatSort;
  displayedColumns: string[] = ['id','name','age','country','state','status'];
  public dataSource;

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

        this.dataSource = new MatTableDataSource(this.applicationPackage.applications);
        this.dataSource.sort = this.sort;
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
      // @ts-ignore
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
      // @ts-ignore
      if (state.filtered) {
        // @ts-ignore
        this.filterOptions.states.push(state.id);
      } else {
        return;
      }
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
