export class RegisterUser {
  userName: string | null = null;
  password: string | null = null;
  passwordConfirm: string | null = null;
  personName: string | null = null;

  constructor(userName: string, password: string, passwordConfirm: string, personName: string) {
    this.userName = userName;
    this.password = password;
    this.passwordConfirm = passwordConfirm;
    this.personName = personName;
  }
}
