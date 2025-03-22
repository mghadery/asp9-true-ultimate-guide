export class LoginUser {
  userName: string | null = null;
  password: string | null = null;

  constructor(userName: string, password: string, passwordConfirm: string, personName: string) {
    this.userName = userName;
    this.password = password;
  }
}
