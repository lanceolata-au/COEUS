import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-loader',
  templateUrl: './app-loader-service.component.html',
  styleUrls: ['./app-loader-service.component.css']
})

export class AppLoaderService {

  @Input()
  public loading: boolean;

}
