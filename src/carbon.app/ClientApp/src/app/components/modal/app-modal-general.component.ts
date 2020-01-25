declare var M: any;
import {AfterViewInit, Component, Input, Output} from '@angular/core';

@Component({
  selector: 'app-modal-general',
  templateUrl: './app-modal-general.component.html',
  styleUrls: ['./app-modal-general.component.css']
})

export class AppModalGeneral implements AfterViewInit {

  @Input()
  public title: string;

  @Input()
  public modalId: string;

  private instance;

  ngAfterViewInit(): void {
    let elems = document.querySelectorAll('#' + this.modalId);
    M.Modal.init(elems, {});
    this.instance = M.Modal.getInstance(elems[0]);
  }

  public static getModalInstance(modalId) {
    let elems = document.querySelectorAll('#' + modalId);
    M.Modal.init(elems, {});
    return M.Modal.getInstance(elems[0]);
  }

}
