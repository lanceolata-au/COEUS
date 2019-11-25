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

  public application = {
    name: null,
    email: null,
    dateOfBirth: new Date()
  };

  private getBlankPreliminaryApplication() {
    this.http.get(getBaseUrl() + "Application/GetBlankPreliminaryApplication").subscribe(
      data => {
        // @ts-ignore
        this.application = data
      },
      error => console.log(error)
    );
  }

  private SubmitApplication() {
    this.http.post(getBaseUrl() + "Application/GetBlankPreliminaryApplication",this.application).subscribe(
      data => {
        console.log(data)
        //this.getBlankPreliminaryApplication();
      },
      error => console.log(error)
    );
  }

}
