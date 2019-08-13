import { Component } from '@angular/core';
import {AfterViewInit} from "@angular/core/src/metadata/lifecycle_hooks";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html'
})


export class CallbackComponent implements AfterViewInit {

  constructor(private activatedRoute: ActivatedRoute) {
    this.activatedRoute.fragment.subscribe(fragment => {

      if (fragment != null) {
        console.log(fragment);
        const response = new URLSearchParams(fragment);

        let test = response.get("id_token");
        console.log(test);
      }

    });
  }

  ngAfterViewInit(): void {
  }

}
