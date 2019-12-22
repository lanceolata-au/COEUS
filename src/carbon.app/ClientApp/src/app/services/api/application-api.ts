import { HttpClient } from "@angular/common/http";
import {config} from "../../config";

export class ApplicationApi {

  private apiEndpoint = "Application/";

  constructor(private http: HttpClient) {
  }

  public submit(application) {

    return this.http.post(config.baseUrl + this.apiEndpoint + "NewPreliminaryApplication", application);

  }
}
