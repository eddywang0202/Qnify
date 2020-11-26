export const Path = {
  GetQuestionTemplate: '/Question/QuestionTemplate',
  DemographicAnswerRequestPath: '/Question/Demographic/Insert',
  GetDemographicQuestionsRequestPath: `/Question/Demographic`,
  GetTestQuestion: (testSetId: number) => `/Question/TestQuestion/${testSetId}`,
  GetActiveTestQuestionList: 'Question/ActiveTestQuestionList',
  InsertTestQuestion: '/Question/TestQuestion/Insert',
  UpdateTestQuestion: '/Question/TestQuestion/Update',
  GetTestQuestionList: (testId: number) => `/Question/TestQuestionList/${testId}`,
  GetParticipantTestQuestion: (testSetId: number) => `/Question/ParticipantTestQuestion/${testSetId}`,
  InsertUserTestQuestion: '/Question/UserTestQuestion/Insert',
  DeleteTestQuestion: (testSetId: number) => `/Question/TestQuestion/${testSetId}`,
  UpdateParticipantTestQuestion: '/Question/ParticipantTestQuestion/Update'
}

export interface IGetTestQuestionListResponse {
  tsid: number,
  tst: string,
  tso: number,
  ia: boolean
}

export interface IDemographicAnswerInsertRequest {
  /**
   * Test Id
   */
  // tid: number,
  /**
   * Bond Easy Token Value
   */
  et: string,
  /**
   * User Question Answers
   */
  uqas:
  {
    /**
     * Test Question Id
     */
    tqid: number,
    /**
     * Answer Text
     */
    a: string
  }[]
}

export type IDemographicQuestionsResponse = IQuestion[];

/**
 * Get Test Question By Test Question Id
 */
export interface ITestQuestionResponse {
  tsid: number,
  tst: string,
  tsq: IQuestion[],
  c: ICell[]
}

export enum QuestionType {
  /**
   * Radio Buttons
   */
  RadioButtons = 1,
  /**
   * Radio Buttons with open ended question
   */
  RadioButtonsWithOpenEndedQuestion,
  /**
   * Checkboxes
   */
  CheckBoxes,
  /**
   * Text Field
   */
  NormalTextField,
  /**
   * Radio Buttons - Optional
   */
  RadioButtonsOptional
}

export enum IQuestionGroupType {
  /**
   * Demographic Question Group
   */
  DemographicQuestionGroup = 1,
  /**
   * Test Set Question Group
   */
  TestSetQuestionGroup = 2,
  /**
   * Test Case Question Group
   */
  TestCaseQuestionGroup = 3,
}

export interface IQuestion {
  /**
   * Test Question Id
   */
  tqid: number,
  /**
   * Question Id
   */
  qid: number,
  /**
   * Question Title
   */
  qt: string,
  /**
   * Cell Id
   */
  cid: number,
  /**
   * Question Group Id
   */
  qgid: IQuestionGroupType,
  /**
   * Question Type
   */
  qtid: QuestionType,
  /**
   * Question Parent Id
   */
  qpid: number,
  /**
   * Question Order
   */
  qo: number,
  /**
   * Answers
   */
  /**
   * 
   */
  a: {
    /**
     * Answer Id
     */
    aid: number,
    /**
     * Answer Title
     */
    at: string,
    /**
     * Next Question Id
     */
    nqid: number,
    /**
     * Answer Order
     */
    ao: number,
    ica: boolean
  }[]
}

export interface IDemographicQuestion extends IQuestion {
  /**
     * Test Question Id
     */
  tqid: number,
}


export interface ICell {
  cid: number,
  cellr: number,
  celll: number,
  cimg: string,
  cpjson: string,
  acpjson: string,
}

export interface IInsertTestQuestionCellRequest {
  cid: number,
  cimg: string,
  acpjson: string,
  forceRefreshId: number
}

export interface IInsertTestQuestionRequest {
  tid: number,
  tst: string,
  tsq: {
    tqid: number,
    cid: number,
    qid: number,
    aid: number[]
  }[],
  c: IInsertTestQuestionCellRequest[]
}

export interface IUpdateTestQuestionRequest extends IInsertTestQuestionRequest {
  tsid: number
}

export interface IUpdateParticipantTestQuestionRequest {
  uqas: {
    tqid: number,
    aid: number[]
  }[]
}