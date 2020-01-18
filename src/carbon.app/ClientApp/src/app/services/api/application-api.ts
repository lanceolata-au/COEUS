import {HttpClient} from "@angular/common/http";
import {config} from "../../config";

export class ApplicationApi {

  private apiEndpoint = "Application/";

  constructor(private http: HttpClient, private loading: boolean) {
  }

  public getApplicationsPackage(filter) {
    return this.http.get(config.baseUrl + this.apiEndpoint + "GetApplicationsPackage", filter).pipe();
  }

  public getNew() {

    return this.http.get(config.baseUrl + this.apiEndpoint + "GetBlankPreliminaryApplication").pipe();
  }

  public submit(application) {

    return this.http.post(config.baseUrl + this.apiEndpoint + "NewPreliminaryApplication", application).pipe();
  }

}
