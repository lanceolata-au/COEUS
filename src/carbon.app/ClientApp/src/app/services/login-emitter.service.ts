import {EventEmitter, Injectable} from "@angular/core";

@Injectable({
  providedIn: "root"
})

export class LoginEmitterService {

  constructor(private emitter: EventEmitter<string>) {

  }

  public loginEvent() {
    this.emitter.emit("loggedIn");
  }

  public subscribeToLogin(callback) {
    this.emitter.subscribe(()=>callback);
  }

}
