import { Component } from '@angular/core';
import { AccountService } from './services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  constructor(public accountService: AccountService, private router: Router) {

  }

  onLogoutClicked() {
    this.accountService.getLogout().subscribe({
      next: () => {
        this.accountService.CurrentUserName = null;
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("token");
        this.router.navigate(['/login']);
      },
      error: (error: any) => { console.log(error); },
      complete: () => { }
    })
  }
}
