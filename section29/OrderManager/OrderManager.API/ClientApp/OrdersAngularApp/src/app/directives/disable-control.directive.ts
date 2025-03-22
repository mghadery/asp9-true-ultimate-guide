import { Directive, Input } from '@angular/core';  // ✅ Import Input
import { FormControl, NgControl } from '@angular/forms';
@Directive({
  selector: '[disableControl]',
  standalone: false
})
export class DisableControlDirective {

  constructor(private ngControl: NgControl) { }

  @Input() set disableControl(condition: boolean) {
    var control = this.ngControl.control as FormControl;
    if (control) {
      if (condition)
        control.disable();
      else
        control.enable();
    }
  }
}
