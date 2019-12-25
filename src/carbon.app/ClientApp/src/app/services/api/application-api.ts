import { HttpClient } from "@angular/common/http";
import { config } from "../../config";

export class ApplicationApi {

  private apiEndpoint = "Application/";

  constructor(private http: HttpClient, private loading: boolean) {
  }

  public getNew() {

    let obj = this.http.get(config.baseUrl + this.apiEndpoint + "GetBlankPreliminaryApplication").pipe();

    return obj;
  }

  public submit(application) {

    let obj = this.http.post(config.baseUrl + this.apiEndpoint + "NewPreliminaryApplication", application).pipe();

    return obj;
  }

}
