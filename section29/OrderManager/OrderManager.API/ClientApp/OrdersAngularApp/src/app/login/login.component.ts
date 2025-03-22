import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AccountService } from '../services/account.service';
import { Validators, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoginFormSubmitted: boolean = false;

  constructor(private accountService: AccountService, private router: Router) {
    this.loginForm = new FormGroup({
      userName: new FormControl(null, []),
      password: new FormControl(null, []),
    });
  }

  get loginForm_userName(): any {
    return this.loginForm.controls['userName'];
  }
  get loginForm_password(): any {
    return this.loginForm.controls['password'];
  }

  loginFormSubmitted() {
    console.log(this.loginForm.value);
    this.isLoginFormSubmitted = true;
    if (this.loginForm.invalid) return;
    console.log('post login ...');
    this.accountService.postLogin(this.loginForm.value)
      .subscribe({
        next: (response: any) => {
          this.loginForm.reset();
          this.isLoginFormSubmitted = false;
          this.accountService.CurrentUserName = response.userName;
          localStorage["token"] = response.token;
          localStorage["refreshToken"] = response.refreshToken;          
          this.router.navigate(['/orders']);
        },
        error: (error: any) => { console.log(error); },
        complete: () => { }
      })
  }
}
