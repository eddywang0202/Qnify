export const Path = {
  GetUserDemographicInfo: (testId: number, userId: number) => `/User/Demographic/${testId}/${userId}`,
}

export interface GetUserDemographicInfoResponse {
  /**
   * Username
   */
  un: string,
  /**
   * Demographic questions and answers
   */
  tsq: 
    {
      "qt": string,
      "a": string[]
    }[]
}