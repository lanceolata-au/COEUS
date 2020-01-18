import {HttpClient} from "@angular/common/http";
import {config} from "../../config";

export class AdminApi {

  private apiEndpoint = "Admin/";

  constructor(private http: HttpClient, private loading: boolean) {
  }

  public getApplicationsPackage(filter) {
    return this.http.post(config.baseUrl + this.apiEndpoint + "GetApplicationsPackage", filter).pipe();
  }

}
