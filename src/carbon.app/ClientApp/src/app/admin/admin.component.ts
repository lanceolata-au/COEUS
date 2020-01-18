import {HttpClient} from "@angular/common/http";
import { applicationStatusLabel } from "./applicationStatusLabel";

declare var M: any;
import {Component, OnInit} from '@angular/core';
import {config} from "../config";
import {AdminApi} from "../services/api/admin-api";

@Component({
  selector: 'app-admin-component',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  private adminApi;

  constructor(private http: HttpClient) {
    this.adminApi = new AdminApi(http, this.loading);
  }

  public loading = false;

  public tabNo = 1;

  public tab(no) { this.tabNo = no; }

  ngOnInit(): void {
    const elem = document.querySelector('.tabs');
    const options = {};
    M.Tabs.init(elem, options);

    const elemsSelect = document.querySelectorAll('select');
    const optionsSelect = {};
    M.FormSelect.init(elemsSelect, optionsSelect);

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

  public applicationPackage = {
    applicationCount: 0,
    applications: [],
    applicationCountries: [],
    applicationStates: []
  };
  public applications = [];

  public getApplications() {
    this.adminApi.getApplicationsPackage(this.filterOptions).subscribe(data => {
      // @ts-ignore
      this.applicationPackage = data;
      this.applicationPackage.applications.forEach(application => {
        application.statusLabel = applicationStatusLabel.get(application.status);
      });
      this.getCountries();
    });
  }

  public countries = [];

  private getCountries() {
    this.loading = true;
    this.http.get(config.baseUrl + "Application/GetCountries").subscribe(
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
    this.http.get(config.baseUrl + "Application/GetStates").subscribe(
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

  public setPage(no) {
    this.filterOptions.page = no;
    this.getApplications();
  }

}
