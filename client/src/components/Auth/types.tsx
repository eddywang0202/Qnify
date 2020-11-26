export interface IUserIdentity {
  name?: string,
  role: IRole
}

export interface IAuth {
  setAccessToken: (token: string) => void,
  getAccessToken: () => string,
  setRefreshToken: (token: string) => void,
  resetAllToken: () => void,
  authenticate: (username: string, password: string) => Promise<boolean>,
  authenticateEzToken: (userInputToken: string) => Promise<boolean>,
  isAuthenticated: boolean,
  setIdentity: (token: string) => IUserIdentity,
  getIdentity: () => IUserIdentity
}

export interface IJwtModel {
  id: number,
  name: string,
  role: IRole,
  exp: number
}

export interface IRefreshJwtModel {
  exp: number
}

export type IRole = 'admin' | 'member' | 'guest';