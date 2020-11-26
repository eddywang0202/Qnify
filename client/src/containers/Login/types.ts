export interface ILoginProp {
  
}

export interface ILoginState {
  authorized: boolean,
  username: string,
  password: string,
  isLoggingIn: boolean,
  errorMessage: string
}