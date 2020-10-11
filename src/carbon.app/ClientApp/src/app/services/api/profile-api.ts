import {HttpClient} from "@angular/common/http";
import {config} from "../../config";
import {ApplicationApi} from "./app-api";

export class ProfileApi {

  private apiEndpoint = "api/profile/";
  private appApi;

  constructor(private http: HttpClient) {
    this.appApi = new ApplicationApi(http);
  }

  public getMyFull() {
    return this.http.get(config.baseUrl + this.apiEndpoint + "my-full").pipe();
  }

  public getProfile() {

    let loggedIn = false;
    let profile = '';

    let profileJson = sessionStorage.getItem("profile");

    if (profileJson != null && profileJson != "") {

      profile = JSON.parse(profileJson);

      this.appApi.getExternalProfile().subscribe(
        data => {
          if (JSON.stringify(data) == profileJson) {
            loggedIn = true;
          } else {
            sessionStorage.setItem("profile", "");
          }
        },
        error => {
          sessionStorage.setItem("profile", "");
          console.log(error);
        }
      );

    }

    return {
      loggedIn : loggedIn,
      profile: profile
    }

  }

}
