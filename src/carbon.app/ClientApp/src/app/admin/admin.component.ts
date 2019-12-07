import {HttpClient} from "@angular/common/http";

declare var M: any;
import {Component, OnInit} from '@angular/core';
import {getBaseUrl} from "../../main";

@Component({
  selector: 'app-admin-component',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  public currentCount = 0;

  constructor(private http: HttpClient) {

  }

  ngOnInit(): void {
    this.getUsers();
    const elem = document.querySelector('.tabs');
    const options= {};
    M.Tabs.init(elem, options)
  }

  public users = {};

  public getUsers() {
    this.http.get(getBaseUrl() + "Admin/GetUsers").subscribe(data => {
      this.users = data;
    });
  }
}
