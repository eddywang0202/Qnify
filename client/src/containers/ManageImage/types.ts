import { ITestQuestionResponse, IQuestion, ICell, IGetTestQuestionListResponse, IInsertTestQuestionRequest, IInsertTestQuestionCellRequest } from 'interfaces/apis/question';
import { PayloadedAction, Action } from 'interfaces/actions';
import * as Actions from './action';
import { IGetTestInfoResponse } from 'interfaces/apis/test';

export interface IManageImageState {
  testList: IGetTestInfoResponse[],
  selectedTestId: number,
  testSetList: IGetTestQuestionListResponse[],
  testSetTitle: string,
  testSetQuestions: IQuestion[],
  cells: ICell[],
  selectedTestSetId?: number,
  formData: IInsertTestQuestionRequest,
  isModalOpened: boolean,
  modal: IImageModal,
  isFetching: boolean,
  isCreateOrUpdating: boolean,
  onCreateNew: boolean,
  hideRowsIndices: number[]
}

export interface IImageModal {
  header: string,
  questions?: IQuestion[],
  imageUrl: string,
  brightness?: number,
  contrast?: number,
  zoomLevel?: number,
  info?: string[]
}

export interface GetTestList extends PayloadedAction<"GET_TEST_LIST", IGetTestInfoResponse[]> { }
export interface SetTestId extends PayloadedAction<"SET_SELECTED_TEST_ID", number> { }
export interface GetTestSetList extends PayloadedAction<"GET_TEST_SET_LIST", IGetTestQuestionListResponse[]> { }
export interface GetTestQuestion extends PayloadedAction<"GET_TEST_SET_QUESTION", ITestQuestionResponse> { }
export interface OnTestQuestionDeleted extends PayloadedAction<"ON_TEST_QUESTION_DELETED", number> { }
export interface OnTestTitleChanged extends PayloadedAction<"ON_TEST_TITLE_CHANGED", string> { }
export interface OnAnswerSelected extends PayloadedAction<"ON_ANSWER_SELECTED", IInsertTestQuestionRequest> { }
export interface OnImageIdChanged extends PayloadedAction<"ON_IMAGE_ID_CHANGED", IInsertTestQuestionCellRequest[]> { }
export interface OnImageUrlChanged extends PayloadedAction<"ON_IMAGE_URL_CHANGED", IInsertTestQuestionCellRequest[]> { }
export interface ResetOnFetchTestQuestion extends Action<"RESET_ON_FETCH_TEST_QUESTION"> { }
export interface ToggleChildVisiblityAction extends PayloadedAction<"TOGGLE_CHILD_VISIBILITY", number> { }
export interface SetModalAction extends PayloadedAction<"SET_MODAL", IImageModal> { }
export interface ResetModalAction extends Action<"RESET_MODAL"> { }
export interface ResetModalAdjustmentAction extends Action<"RESET_MODAL_ADJUSTMENT"> { }
export interface ToggleTestQuestionIsFetching extends PayloadedAction<"TOGGLE_TEST_QUESTION_IS_FETCHING", boolean> { }
export interface ToggleTestQuestionIsCreateOrUpdating extends PayloadedAction<"TOGGLE_TEST_QUESTION_IS_CREATE_OR_UPDATING", boolean> { }
export interface OnCreateNewTestQuestion extends Action<"ON_CREATE_NEW_TEST_QUESTION"> { }
export interface SubmitFormData extends Action<"SUBMIT_NEW_TEST_QUESTION_FORM"> { }
export interface SetSelectedTestSetID extends PayloadedAction<'SET_SELECTED_TEST_SET_ID', number> { }
export interface HideRowIndices extends PayloadedAction<'HIDE_ROW_INDICES', number[]> { }

export interface ChangeModalImageBrightnessAction extends PayloadedAction<"CHANGE_MODAL_IMAGE_BRIGHTNESS", number> { }
export interface ChangeModalImageContrastAction extends PayloadedAction<"CHANGE_MODAL_IMAGE_CONTRAST", number> { }
export interface ChangeModalImageZoomLevelAction extends PayloadedAction<"CHANGE_MODAL_IMAGE_ZOOM_LEVEL", number> { }

export type ManageImageActions = 
  GetTestSetList | GetTestQuestion | ResetOnFetchTestQuestion | ToggleTestQuestionIsFetching | OnCreateNewTestQuestion | OnImageIdChanged | SetSelectedTestSetID | OnImageUrlChanged |
  ToggleChildVisiblityAction | SetModalAction | ResetModalAction | ChangeModalImageBrightnessAction | OnTestTitleChanged | SubmitFormData | GetTestList | SetTestId | HideRowIndices |
  ChangeModalImageContrastAction | ChangeModalImageZoomLevelAction | ResetModalAdjustmentAction | OnAnswerSelected | OnTestQuestionDeleted | ToggleTestQuestionIsCreateOrUpdating;

export type IManageImage = IManageImageState & typeof Actions;