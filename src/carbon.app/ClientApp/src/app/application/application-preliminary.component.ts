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
     this.getStates();
    }
  constructor(private http: HttpClient) {
  }

  ngAfterViewInit(): void {
    const elems_modal = document.querySelectorAll('.modal');
    const instances_modal = M.Modal.init(elems_modal, {});
  }

  public application = {
    name: null,
    email: null,
    dateOfBirth: null
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
      error => console.log(error)
    );
  }

  public countries = {

  };

  private getCountries() {
    this.http.get(getBaseUrl() + "Application/GetCountries").subscribe(
      data => {
        // @ts-ignore
        this.countries = Object.values(data);
      },
      error => console.log(error)
    );
  }

  public states = {

  };

  private getStates() {
    this.http.get(getBaseUrl() + "Application/GetStates").subscribe(
      data => {
        // @ts-ignore
        this.states = Object.values(data);
      },
      error => console.log(error)
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
      error => console.log(error)
    );
  }



}
