import { Component } from '@angular/core';
import {config} from "../config";

@Component({
  selector: 'app-nav-footer',
  templateUrl: './nav-footer.component.html',
  styleUrls: ['./nav-footer.component.css']
})
export class NavFooterComponent {

  public version = config.version;

}
