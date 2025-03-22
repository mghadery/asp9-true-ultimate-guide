import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AccountService } from '../services/account.service';
import { Validators, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm: FormGroup;
  isRegisterFormSubmitted: boolean = false;

  constructor(private accountService: AccountService, private router: Router) {
    this.registerForm = new FormGroup({
      userName: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required]),
      passwordConfirm: new FormControl(null, [Validators.required]),
      personName: new FormControl(null, [Validators.required])
    });
  }

  get registerForm_userName(): any {
    return this.registerForm.controls['userName'];
  }
  get registerForm_password(): any {
    return this.registerForm.controls['password'];
  }
  get registerForm_passwordConfirm(): any {
    return this.registerForm.controls['passwordConfirm'];
  }
  get registerForm_personName(): any {
    return this.registerForm.controls['personName'];
  }

  registerFormSubmitted() {
    this.isRegisterFormSubmitted = true;
    if (this.registerForm.invalid) return;
    this.accountService.postRegister(this.registerForm.value)
      .subscribe({
        next: (response: any) => {
          this.registerForm.reset();
          this.isRegisterFormSubmitted = false;
          this.accountService.CurrentUserName = response.userName;
          localStorage["token"] = response.token;
          localStorage["refreshToken"] = response.refreshToken;          
          this.router.navigate(['/orders']);
        },
        error: (error: any) => { console.log(error); },
        complete: () => {}
      })
  }
}
