declare var M: any;
import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-admin-component',
  templateUrl: './admin.component.html'
})
export class AdminComponent implements OnInit {
  public currentCount = 0;

  ngOnInit(): void {
    const elem = document.querySelector('.tabs');
    const options= {};
    M.Tabs.init(elem, options)
  }
}
