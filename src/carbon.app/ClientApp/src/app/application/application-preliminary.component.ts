import {AfterViewInit, Component, OnInit} from '@angular/core';
import * as M from 'materialize-css';
import {getBaseUrl} from "../../main";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-application-preliminary-component',
  templateUrl: './application-preliminary.component.html'
})
export class ApplicationPreliminaryComponent implements OnInit, AfterViewInit {

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
    this.http.get(getBaseUrl() + "Application/GetBlankPreliminaryApplication").subscribe(
      data => {
        // @ts-ignore
        this.application = data;

      },
      error => {
        console.log(error);

        M.toast({html: error.error, classes: "rounded red"});

      }
    );
  }

  public countries = [];

  private getCountries() {
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

      }
    );
  }

  public states = [];

  private getStates() {
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
          this.states = this.states.concat([stateList]);
        });

      },
      error => {
        console.log(error);

        M.toast({html: error.error, classes: "rounded red"});

      }
    );
  }

  private SubmitApplication() {
    console.log(this.application);

    this.application.dateOfBirth = new Date(this.dateOfBirth.year, this.dateOfBirth.month - 1, this.dateOfBirth.day);

    this.http.post(getBaseUrl() + "Application/NewPreliminaryApplication",this.application).subscribe(
      data => {
        console.log(data);
        window.location.reload();
        //this.getBlankPreliminaryApplication();
      },
      error => {
        console.log(error);

        M.toast({html: error.error, classes: "rounded red"});

      }

    );
  }



}
