import {AfterViewChecked, Component, OnInit} from '@angular/core';
import {ProfileApi} from "../services/api/profile-api";
import {HttpClient} from "@angular/common/http";

import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.css']
})
export class ActivitiesComponent implements OnInit, AfterViewChecked {

  public loading = false;
  public readyToRender = false;

  private profileApi;



  activitiesSelectedDay01 = [
  ];

  activitiesSelectedDay02 = [
  ];

  activitiesSelectedDay03 = [
  ];

  activitiesSelectedDay04 = [
  ];

  activitiesAvailable = [
    'placeholder 00',
    'placeholder 01',
    'placeholder 02',
    'placeholder 03',
    'placeholder 04'
  ];

  expeditionsSelected = [
  ];

  expeditionsAvailable = [
    'Mountain Biking',
    'Hiking',
    'West coast explorer',
    'East coast explorer',
    'Island explorer',
    'King Island',
    'Taste of Tasmania/ Mastication tour'
  ];

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
    }
  }

  constructor(private http: HttpClient) {
    this.profileApi = new ProfileApi(http);
  }

  public profile = {
    userName: null,
    coreUserDto: {
      access: 0,
      picture: null
    }
  };

  ngOnInit() {

    this.profile = this.profileApi.getProfile().profile;

    this.getProfile();
  }

  private getProfile() {



    let profileJson = sessionStorage.getItem("profile");

    if (profileJson != null) {

      this.profile = JSON.parse(profileJson);

    }

  }

  ngAfterViewChecked(): void {
    this.readyToRender = true;
  }

}
