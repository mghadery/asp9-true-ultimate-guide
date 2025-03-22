import { Injectable } from '@angular/core';
import { RegisterUser } from '../models/register-user'
import { LoginUser } from '../models/login-user'
import { UserInfo } from '../models/user-info'
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const API_BASE_URL: string = "https://localhost:7121/api/ver1/account/";

@Injectable({
  providedIn: 'root'
})

export class AccountService {
  public CurrentUserName: string | null = null;
  constructor(private httpClient: HttpClient) {
  }
  public postRegister(user: RegisterUser): Observable<any> {
    return this.httpClient.post<any>(`${API_BASE_URL}register`, user);
  }
  public postLogin(user: LoginUser): Observable<any> {
    return this.httpClient.post<any>(`${API_BASE_URL}login`, user);
  }
  public getLogout(): Observable<string> {
    return this.httpClient.get<string>(`${API_BASE_URL}logout`);
  }
  public postRefresh(): Observable<any> {
    let jwtToken = localStorage["token"];
    let refreshToken = localStorage["refreshToken"];
    let tokens = { jwtToken, refreshToken };
    return this.httpClient.post<any>(`${API_BASE_URL}getnewtoken`, tokens);
  }
}
