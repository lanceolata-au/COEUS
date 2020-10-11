import {HttpClient} from "@angular/common/http";
import {config} from "../../config";

export class ApplicationApi {

  private apiEndpoint = "api/app/";

  constructor(private http: HttpClient) {
  }

  public getExternalProfile() {
    return this.http.get(config.baseUrl + this.apiEndpoint + "externalProfile").pipe();
  }

}
