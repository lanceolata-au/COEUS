import {AfterViewInit, Component, OnInit} from '@angular/core';
import * as M from 'materialize-css';
import {getBaseUrl} from "../../main";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-application-preliminary-component',
  templateUrl: './application-preliminary.component.html'
})
export class ApplicationPreliminaryComponent implements OnInit {

  ngOnInit(): void {
     this.getBlankPreliminaryApplication();

    document.addEventListener('DOMContentLoaded', function() {

      const elems = document.querySelectorAll('.datepicker');

      const options = {
        yearRange: 40,
        defaultDate: new Date(1980, 1),
        format: "dd mmm yyyy"
      };

      const instances = M.Datepicker.init(elems, options);

    });
    }
  constructor(private http: HttpClient) {
  }

  private profile = {
    userName: null,
    coreUserDto: {
      id: null,
      access: 0,
      picture: null
    }
  };

  private status;

  private getBlankPreliminaryApplication() {
    this.http.get(getBaseUrl() + "Application/GetBlankPreliminaryApplication").subscribe(
      data => {
        this.status = data
      },
      error => console.log(error)
    );
  }

}
