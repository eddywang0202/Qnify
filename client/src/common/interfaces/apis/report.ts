export const Path = {
  GetParticipantResult: (testId: number) => `Report/Participant/${testId}`,
  GetParticipantReport: 'Report/Participant/TestResultList',
  GetParticipantDetailReport: (testId: number, userId: number) => `Report/Participant/TestResultDetail/${testId}/${userId}`
}

export interface IGetParticipantResultResponse {
  qtc: number,
  catc: number,
  rp: IParticipantDetailResultPerformance[],
}

export interface IGetParticipantReportRequest {
  o: number,
  l: number,
  un: string,
  tid?: number
}

export interface IParticipantReportList {
  /**
   * Username
   */
  un: string,
  /**
   * User Id
   */
  uid: number,
  /**
   * Submit Date
   */
  sd: number,
  /**
   * Test Name
   */
  tn: string,
  /**
   * Test Id
   */
  tid: number,
  /**
   * Total Correct Question
   */
  tcq: number,
  /**
   * Total Answered Question
   */
  taq: number,
  /**
   * Total Question
   */
  tq: number
}

export interface IGetParticipantReportResponse {
  /**
   * Total Page Count
   */
  tpc: number,
  /**
   * User Test Question Report
   */
  utqr: IParticipantReportList[]
}

export type IGetParticipantDetailReportRequest = number;
export type IGetParticipantDetailReportResponse = IParticipantDetailReport;

export interface IParticipantDetailTestSetReport {
  tsid: number,
  tst: string,
  tsq: {
    tqid: number,
    qid: number,
    cid: number,
    qt: string,
    qtid: number,
    qgid: number,
    qpid: number,
    qo: number,
    a: string[],
    ca: [
      string
    ],
    ic: boolean
  }[]
  ,
  c: {
    cid: number,
    cellr: number,
    celll: number,
    cimg: string,
    cpjson: string,
    acpjson: string
  }[]
}

export interface IParticipantDetailResultPerformance {
  rpn: string,
  rpr: number,
  is: boolean
}

export interface IParticipantDetailReport {
  /**
  * Test Set Report
  */
  rsm: IParticipantDetailTestSetReport[],
  /**
   * Result Performance
   */
  rp: IParticipantDetailResultPerformance[]
}