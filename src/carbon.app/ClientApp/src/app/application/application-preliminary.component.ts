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

    }
  constructor(private http: HttpClient) {
  }

  ngAfterViewInit(): void {
    const elems_datepick = document.querySelectorAll('.datepicker');
    const options = {
      yearRange: 40,
      defaultDate: new Date(1980, 1),
      format: "dd mmm yyyy"
    };
    const instances_datepick = M.Datepicker.init(elems_datepick, options);

    var elems_modal = document.querySelectorAll('.modal');
    var instances_modal = M.Modal.init(elems_modal, {});
  }

  public application = {
    name: null,
    email: null,
    dateOfBirth: null
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

  private SubmitApplication() {
    console.log(this.application);
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
