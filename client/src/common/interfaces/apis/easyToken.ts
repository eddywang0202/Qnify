export const Path = {
  EasyTokenGenerate: '/EasyToken/Generate',
  EasyTokenExtend: `/EasyToken/Extend`,
  EasyTokenGet: '/EasyToken/Get',
  EasyTokenVerify: '/EasyToken/Validate',
  EasyTokenRemove:'/EasyToken/Remove',
}

export interface IEasyTokenGetResponse {
  /**
   * User Id
   */
  uid: number,
  /**
   * Username
   */
  un: string,
  /**
   * Easy Token
   */
  etv: string,
  /**
   * Expires
   */
  e: number
}

export type IEasyTokenGenerateResponse = IEasyTokenGetResponse;

export type IEasyTokensGetResponse = IEasyTokenGetResponse[];

export interface IEasyTokenExtendResponse {
  /**
   * User Id
   */
  uid: number,
  /**
   * Username
   */
  un: string,
  /**
   * Easy Token Value
   */
  etv: string,
  /**
   * Expire
   */
  e: number
}
