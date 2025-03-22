export class UserInfo {
  userName: string | null = null;
  personName: string | null = null;

  constructor(userName: string, personName: string) {
    this.userName = userName;
    this.personName = personName;
  }
}
