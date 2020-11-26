import { number, string } from "prop-types";

export const Path = {
  GetAllTestInfo: 'Test/List',
  GetActiveTestInfo: 'Test',
  InsertTest: 'Test/Insert',
  DeleteTest: (testId: number) => `Test/${testId}`,
  UpdateTestConsent: (testId: number) => `Test/Consent/Update/${testId}`,
  GetTestConsent: (testId: number) => `Test/Consent/${testId}`,
  UpdateTestStatus: (testId: number) => `Test/Status/Update/${testId}`,
  UpdateTestTitle: (testId: number) => `Test/Title/Update/${testId}`,
}

export type IInsertTestRequest = string;
export type IInsertTestResponse = IGetTestInfo;

export type IDeleteTestResponse = boolean;

export type IUpdateConsentRequest = string;
export type IUpdateConsentResponse = boolean;
export type IGetConsentResponse = string;

export type IGetTestInfoResponse = IGetTestInfo;
export interface IGetTestInfo {
  /**
   * Test Id 
   */
  tid: number,
  /**
   * Test Title 
   */
  t: string,
  /**
   * Consent HTML string 
   */
  s: boolean
}

export type IUpdateTestTitleRequest = string;
export type IUpdateTestTitleResponse = boolean;
export type IUpdateTestStatusRequest = boolean;
export type IUpdateTestStatusResponse = IGetTestInfo;