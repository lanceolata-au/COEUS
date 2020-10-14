import {AfterViewInit, Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ApplicationInformation} from "../services/strings/applicationInformation";
import {ApplicationApi} from "../services/api/application-api";
import {DateHelper} from "../services/helpers/date-helper";
import {MatSnackBar} from "@angular/material/snack-bar";

import * as M from "../../assets/materializescss/js/compiled/materialize.js";

@Component({
  selector: 'app-application-preliminary-component',
  templateUrl: './application-preliminary.component.html'
})
export class ApplicationPreliminaryComponent implements OnInit, AfterViewInit {

  public loading = false;
  public applicationSubmitted = false;

  constructor(private http: HttpClient,private _snackBar: MatSnackBar) {
    this.applicationApi = new ApplicationApi(http);
  }

  public startDate = new Date(2000, 0, 0);

  public applicationAgeAtMoot = {
    years: null,
    months: null
  };

  private applicationApi: ApplicationApi;

  public TOS = ApplicationInformation.TOS;

  private openSnackBarError(message:string) {
    this._snackBar.open(message, "", {
      duration: 5000,
      panelClass: 'aim-red'
    })
  }

  private openSnackBarInfo(message:string) {
    this._snackBar.open(message, "", {
      duration: 5000,
    })
  }

  ngOnInit(): void {

    this.loading = true;

    if (localStorage.getItem("application") !== null) {
      this.application = JSON.parse(localStorage.getItem("application"));
      this.applicationSubmitted = true;
      this.dateFix();
    } else {
      this.getBlankPreliminaryApplication();
    }

    this.getCountries();

  }

  private instances_modal;

  ngAfterViewInit(): void {
    const elems_modal = document.querySelectorAll('.modal');
    this.instances_modal = M.Modal.init(elems_modal, {});

    const elems_select = document.querySelectorAll('select');
    this.elems_select = elems_select;
    M.FormSelect.init(elems_select);
  }

  private elems_select;

  public application = {
    name: null,
    email: null,
    dateOfBirth: null,
    country: 0,
    state: 0
  };

  public dateOfBirth = {
    day: null,
    month: null,
    year: null
  };

  public dateOfBirthRaw;

  private getBlankPreliminaryApplication() {
    this.loading = true;
    this.application.country = 1;
    this.application.state = 48;
    localStorage.removeItem("application");


    this.applicationApi.getNew().subscribe(
      data => {
        // @ts-ignore
        this.application = data;

        this.application.country = 1;
        this.application.state = 48;
        this.dateOfBirth.day = null;
        this.dateOfBirth.month = null;
        this.dateOfBirth.year = null;
        this.applicationSubmitted = false;
        this.loading = false;
      },
      error => {
        console.log(error);

        this.openSnackBarError(error.error);

        this.loading = false;
      }
    );
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

        this.openSnackBarError(error.error);

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

        this.openSnackBarError(error.error);

        this.loading = false;
      }
    );
  }

  public countryChange() {
    let state = this.states[this.application.country - 1][0];
    this.application.state = state.id;
  }

  private dateFix() {
    let dob = new Date(this.application.dateOfBirth);
    let mootStart = new Date(2022, 12, 31, 0);
    this.applicationAgeAtMoot = DateHelper.daysBetween(dob, DateHelper.mootStart);

    this.dateOfBirth.day = dob.getDay() + 1;
    this.dateOfBirth.month = dob.getMonth() + 1;
    this.dateOfBirth.year = dob.getFullYear();
  }

  public SubmitApplication() {

    this.loading = true;

    this.instances_modal[0].close();

    this.application.dateOfBirth = new Date(this.dateOfBirthRaw);

    let tzOffset = this.application.dateOfBirth.getTimezoneOffset();

    this.application.dateOfBirth = new Date(this.application.dateOfBirth.getTime() + tzOffset * 60000 + 24*60*60000);

    this.applicationApi.submit(this.application)
      .subscribe(data => {
        console.log(data);
        this.openSnackBarInfo("Successfully Submitted!");
        this.loading = false;
        localStorage.setItem('application', JSON.stringify(this.application));
        this.dateFix();
        this.applicationSubmitted = true;
      }, error => {
        console.log(error);

        this.openSnackBarError(error.error);

        this.loading = false;
    });

  }



}
