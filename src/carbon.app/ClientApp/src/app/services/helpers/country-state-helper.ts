import { HttpClient } from "@angular/common/http";
import { ApplicationApi } from "../api/application-api";
import {EventEmitter, Output} from "@angular/core";

export class CountryStateHelper {

  private applicationApi;
  private loading;

  constructor(private http: HttpClient, isAsync: boolean = true) {
    this.applicationApi = new ApplicationApi(http, this.loading);
  }

  public getFormatted() {

  }

}

