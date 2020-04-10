import {HttpClient} from "@angular/common/http";
import {config} from "../../config";

export class ApplicationApi {

  private apiEndpoint = "api/application/";

  constructor(private http: HttpClient, private loading: boolean) {
  }

  public getApplicationsPackage(filter) {
    return this.http.get(config.baseUrl + this.apiEndpoint + "applicationsPackage", filter).pipe();
  }

  public getNew() {

    return this.http.get(config.baseUrl + this.apiEndpoint + "preliminaryApplication").pipe();
  }

  public getCountries() {
    return this.http.get(config.baseUrl + this.apiEndpoint + "countries").pipe();
  }

  public getStates() {
    return this.http.get(config.baseUrl + this.apiEndpoint + "states").pipe();
  }

  public submit(application) {
    return this.http.post(config.baseUrl + this.apiEndpoint + "preliminaryApplication", application).pipe();
  }

}
