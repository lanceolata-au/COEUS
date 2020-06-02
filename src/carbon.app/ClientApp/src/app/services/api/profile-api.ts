import {HttpClient} from "@angular/common/http";
import {config} from "../../config";

export class ProfileApi {

  private apiEndpoint = "api/profile/";

  constructor(private http: HttpClient, private loading: boolean) {
  }

  public getMyFull() {
    return this.http.get(config.baseUrl + this.apiEndpoint + "my-full").pipe();
  }

}
