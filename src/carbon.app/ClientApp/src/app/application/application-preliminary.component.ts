import {AfterViewInit, Component, OnInit} from '@angular/core';
import * as M from 'materialize-css';
import {getBaseUrl} from "../../main";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-application-preliminary-component',
  templateUrl: './application-preliminary.component.html'
})
export class ApplicationPreliminaryComponent implements OnInit, AfterViewInit {

  public loading = false;

  ngOnInit(): void {
     this.getBlankPreliminaryApplication();
     this.getCountries();
    }
  constructor(private http: HttpClient) {
  }

  ngAfterViewInit(): void {
    const elems_modal = document.querySelectorAll('.modal');
    const instances_modal = M.Modal.init(elems_modal, {});

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

  private getBlankPreliminaryApplication() {
    this.loading = true;
    this.http.get(getBaseUrl() + "Application/GetBlankPreliminaryApplication").subscribe(
      data => {
        // @ts-ignore
        this.application = data;
        this.application.country = 1;
        this.application.state = 48;
        this.loading = false;
      },
      error => {
        console.log(error);

        M.toast({html: error.error, classes: "rounded red"});
        this.loading = false;
      }
    );
  }

  public countries = [];

  private getCountries() {
    this.loading = true;
    this.http.get(getBaseUrl() + "Application/GetCountries").subscribe(
      data => {
        // @ts-ignore
        this.countries = Object.values(data);
        M.FormSelect.init(this.elems_select);
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
    this.http.get(getBaseUrl() + "Application/GetStates").subscribe(
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
        });
        this.loading = false;
      },
      error => {
        console.log(error);

        M.toast({html: error.error, classes: "rounded red"});
        this.loading = false;
      }
    );
  }

  public countryChange() {
    let state = this.states[this.application.country - 1][0];
    this.application.state = state.id;
  }

  public SubmitApplication() {

    this.loading = true;

    this.application.dateOfBirth = new Date(this.dateOfBirth.year, this.dateOfBirth.month - 1, this.dateOfBirth.day);

    this.http.post(getBaseUrl() + "Application/NewPreliminaryApplication",this.application).subscribe(
      data => {
        console.log(data);
        M.toast({html: "Successfully Submitted!", classes: "rounded green"});
        this.getBlankPreliminaryApplication();
        this.dateOfBirth = {
          day: null,
          month: null,
          year: null
        };
      },
      error => {
        console.log(error);

        M.toast({html: error.error, classes: "rounded red"});
        this.loading = false;
      }

    );
  }



}
