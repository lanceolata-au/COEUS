import { Component } from '@angular/core';
import * as M from 'materialize-css';
import {AfterViewInit} from "@angular/core/src/metadata/lifecycle_hooks";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})

export class NavMenuComponent implements AfterViewInit {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
    const sidenav = document.querySelectorAll('.sidenav');
    const instance = M.Sidenav.getInstance(sidenav[0]);
    instance.close();
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
    const sidenav = document.querySelectorAll('.sidenav');
    const instance = M.Sidenav.getInstance(sidenav[0]);
    if (this.isExpanded) {
      instance.open();
    } else {
      instance.close();
    }


  }

  ngAfterViewInit(): void {
    const sidenav = document.querySelectorAll('.sidenav');
    M.Sidenav.init(sidenav);
  }
}
