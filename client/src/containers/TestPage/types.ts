import { IQuestion, ICell, ITestQuestionResponse, IGetTestQuestionListResponse, IUpdateParticipantTestQuestionRequest } from "interfaces/apis/question";
import { IImageModal } from "../ManageImage/types";
import { PayloadedAction, Action } from "interfaces/actions";
import * as Actions from './action';

export interface ITestPageState {
  testSetTitle: string,
  selectedTestSetId: number,
  testSetList: IGetTestQuestionListResponse[],
  testSetQuestions: IQuestion[],
  cells: ICell[],
  formData: IUpdateParticipantTestQuestionRequest,
  isModalOpened: boolean,
  modal: IImageModal,
  isFetching: boolean,
  isFormSubmitting: boolean,
  hideRowsIndices: number[]
}

export interface OnGetTestQuestionList extends PayloadedAction<"ON_GET_QUESTION_LIST", IGetTestQuestionListResponse[]> { }
export interface GetTestQuestion extends PayloadedAction<"GET_TEST_QUESTION", ITestQuestionResponse> { }
export interface OnAnswerSelected extends PayloadedAction<"ON_ANSWER_SELECTED", IUpdateParticipantTestQuestionRequest> { }
export interface UpdateTestQuestion extends PayloadedAction<"ON_QUESTION_UPDATED_SELECTED", IQuestion[]> { }
export interface ResetOnFetchTestQuestion extends Action<"RESET_ON_FETCH_TEST_QUESTION"> { }
export interface OnFormSubmit extends PayloadedAction<"ON_FORM_SUBMIT", number> { }
export interface SetModalAction extends PayloadedAction<"SET_MODAL", IImageModal> { }
export interface ResetModalAction extends Action<"RESET_MODAL"> { }
export interface ResetModalAdjustmentAction extends Action<"RESET_MODAL_ADJUSTMENT"> { }
export interface ToggleTestQuestionIsFetching extends PayloadedAction<"TOGGLE_TEST_QUESTION_IS_FETCHING", boolean> { }
export interface ToggleTestQuestionIsSubmitting extends PayloadedAction<"TOGGLE_TEST_QUESTION_IS_SUBMITTING", boolean> { }
export interface SetSelectedTestSetID extends PayloadedAction<'SET_SELECTED_TEST_SET_ID', number> { }
export interface HideRowIndices extends PayloadedAction<'HIDE_ROW_INDICES', number[]> { }


export interface ChangeModalImageBrightnessAction extends PayloadedAction<"CHANGE_MODAL_IMAGE_BRIGHTNESS", number> { }
export interface ChangeModalImageContrastAction extends PayloadedAction<"CHANGE_MODAL_IMAGE_CONTRAST", number> { }
export interface ChangeModalImageZoomLevelAction extends PayloadedAction<"CHANGE_MODAL_IMAGE_ZOOM_LEVEL", number> { }

export type TestQuestionActions =
  OnAnswerSelected | ResetOnFetchTestQuestion | SetModalAction | HideRowIndices | GetTestQuestion | OnFormSubmit | ToggleTestQuestionIsSubmitting |
  ResetModalAction | ResetModalAdjustmentAction | ToggleTestQuestionIsFetching | ChangeModalImageBrightnessAction | UpdateTestQuestion |
  ChangeModalImageContrastAction | ChangeModalImageZoomLevelAction | OnGetTestQuestionList | SetSelectedTestSetID;

export type ITestQuestionPage = ITestPageState & typeof Actions;