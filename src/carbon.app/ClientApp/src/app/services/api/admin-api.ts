import {HttpClient} from "@angular/common/http";
import {config} from "../../config";

export class AdminApi {

  private apiEndpoint = "api/admin/";

  constructor(private http: HttpClient, private loading: boolean) {
  }

  public getApplicationsPackage(filter) {
    return this.http.post(config.baseUrl + this.apiEndpoint + "applicationsPackage", filter).pipe();
  }

  public getUsers(filter) {
    return this.http.get(config.baseUrl + this.apiEndpoint + "users").pipe();
  }

}
