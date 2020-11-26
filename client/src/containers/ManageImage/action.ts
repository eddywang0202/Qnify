import * as Types from './types';
import { Dispatch } from 'redux';
import { GET, POST, DELETE } from 'server';
import { IGetTestQuestionListResponse, ITestQuestionResponse, Path as TestQuestionPath, IInsertTestQuestionRequest, 
  ICell, IInsertTestQuestionCellRequest, IUpdateTestQuestionRequest } from 'interfaces/apis/question';
import { IControlSelectionValue } from '~/components/Controls/types';
import { Path as TestPath, IGetTestInfoResponse } from 'interfaces/apis/test';

export function GetTestList() {
  return async (dispatch: Dispatch<Types.ManageImageActions>) => {

    let resp = await GET<IGetTestInfoResponse[]>(TestPath.GetAllTestInfo);

    dispatch({
      type: 'GET_TEST_LIST',
      payload: resp.data
    });
  }
}

export function GetTestSetList(testId: number) {
  return async (dispatch: Dispatch<Types.ManageImageActions>) => {

    let resp = await GET<IGetTestQuestionListResponse[]>(TestQuestionPath.GetTestQuestionList(testId));

    dispatch({
      type: 'SET_SELECTED_TEST_ID',
      payload: testId
    });

    dispatch({
      type: 'GET_TEST_SET_LIST',
      payload: resp.data
    });
  }
}

export function OnCreateNewTestQuestion() {
  return async (dispatch: Dispatch<Types.ManageImageActions>) => {

    let resp = await GET<ITestQuestionResponse>(TestQuestionPath.GetQuestionTemplate);
    
    dispatch({
      type: 'GET_TEST_SET_QUESTION',
      payload: resp.data
    });
  }
}

export function GetTestQuestion(testSetId: number) {
  return async (dispatch: Dispatch<Types.ManageImageActions>) => {

    dispatch({
      type: 'SET_SELECTED_TEST_SET_ID',
      payload: testSetId
    })

    let resp = await GET<ITestQuestionResponse>(TestQuestionPath.GetTestQuestion(testSetId));

    dispatch({
      type: 'GET_TEST_SET_QUESTION',
      payload: resp.data
    });
  }
}

export function DeleteTestQuestion(testQuestionId: number) {
  return async (dispatch: Dispatch<Types.ManageImageActions>) => {

    try {
      let resp = await DELETE(TestQuestionPath.DeleteTestQuestion(testQuestionId));

      dispatch({
        type: 'ON_TEST_QUESTION_DELETED',
        payload: testQuestionId
      });

      return true;
    }
    catch (err) {
      alert('Failed to delete test');

      return false;
    }
  }
}

export function OnAnswerSelected(selected: IControlSelectionValue) {
  return (dispatch: Dispatch<Types.ManageImageActions>, getState: () => { manageImage: Types.IManageImageState }) => {

    let state = getState().manageImage;

    var newFormData: IInsertTestQuestionRequest = {
      ...state.formData,
      tsq: state.formData.tsq.map(d => {
      if (d.qid === selected.questionId && d.cid === selected.cellId)
        return {
          tqid: selected.testQuestionId,
          cid: selected.cellId,
          qid: selected.questionId,
          aid: Array.isArray(selected.answerId) ? ((d.aid.findIndex(x => x === selected.answerId[0]) < 0) 
            ? [...d.aid, selected.answerId[0]] 
            : d.aid.filter(x => x !== selected.answerId[0]))
            : [selected.answerId]
        }
      else return d
    })
  }

    dispatch({
      type: 'ON_ANSWER_SELECTED',
      payload: newFormData
    })
  }
}

export function OnImageIdChanged(cell: ICell) {
  return (dispatch: Dispatch<Types.ManageImageActions>, getState: () => { manageImage: Types.IManageImageState }) => {

    let state = getState().manageImage;

    let cellRequest: IInsertTestQuestionCellRequest[] = [
      ...(state.formData.c || []).map(x => {
        if (x.cid === cell.cid) {
          x.acpjson = cell.acpjson
        }

        return x;
      }),
    ];

    dispatch({
      type: 'ON_IMAGE_ID_CHANGED',
      payload: cellRequest
    });
  }
}

export function OnImageUrlChanged(cell: ICell) {
  return (dispatch: Dispatch<Types.ManageImageActions>, getState: () => { manageImage: Types.IManageImageState }) => {

    let state = getState().manageImage;

    let cellRequest: IInsertTestQuestionCellRequest[] = [
      ...(state.formData.c || []).map(x => {
        if (x.cid === cell.cid) {
          x.cimg = cell.cimg
          x.forceRefreshId = Date.now()
        }

        return x;
      }),
    ];
    dispatch({
      type: 'ON_IMAGE_URL_CHANGED',
      payload: cellRequest
    });
  }
}

export function SubmitNewForm(formData: IInsertTestQuestionRequest) {
  return async (dispatch: Dispatch<Types.ManageImageActions>) => {

    try {
      let resp = await POST(TestQuestionPath.InsertTestQuestion, formData);

      alert(`Successfully added test ${formData.tst}.`);
    }
    catch(err) {
      console.error(err);
    }
  }
};

export function UpdateForm(formData: IUpdateTestQuestionRequest) {
  return async (dispatch: Dispatch<Types.ManageImageActions>) => {

    try {
      let resp = await POST(TestQuestionPath.UpdateTestQuestion, formData);

      alert(`Successfully updated test ${formData.tst}.`);
    }
    catch (err) {
      console.error(err);
    }
  }
};

export function SetSelectedTestSetId(testSetId: number): Types.ManageImageActions {
  return {
    type: 'SET_SELECTED_TEST_SET_ID',
    payload: testSetId
  }
}

export function HideRowIndices(hideRowsIndices: number[]): Types.ManageImageActions {
  return {
    type: 'HIDE_ROW_INDICES',
    payload: hideRowsIndices
  }
}

export function OnTestTitleChanged(title: string): Types.ManageImageActions {
  return {
    type: 'ON_TEST_TITLE_CHANGED',
    payload: title
  }
}

export function ToggleIsFetching(isFetching: boolean) : Types.ManageImageActions {
  return {
    type: 'TOGGLE_TEST_QUESTION_IS_FETCHING',
    payload: isFetching
  }
}

export function ToggleIsCreateOrUpdating(isCreateOrUpdating: boolean): Types.ManageImageActions {
  return {
    type: 'TOGGLE_TEST_QUESTION_IS_CREATE_OR_UPDATING',
    payload: isCreateOrUpdating
  }
}

export function ResetOnFetchTestQuestion(): Types.ManageImageActions {
  return {
    type: 'RESET_ON_FETCH_TEST_QUESTION',
  }
}

export function ToggleChildVisibility(parentId: number): Types.ManageImageActions {
  return {
    type: 'TOGGLE_CHILD_VISIBILITY',
    payload: parentId
  }
}

export function SetModal(modal: Types.IImageModal): Types.ManageImageActions {
  return {
    type: 'SET_MODAL',
    payload: modal
  }
}

export function ResetModal(): Types.ManageImageActions {
  return {
    type: 'RESET_MODAL',
  }
}

export function ResetModalImageAdjustment(): Types.ManageImageActions {
  return {
    type: 'RESET_MODAL_ADJUSTMENT',
  }
}

export function ChangeModalImageBrightness(value: number): Types.ManageImageActions {
  return {
    type: 'CHANGE_MODAL_IMAGE_BRIGHTNESS',
    payload: value
  }
}

export function ChangeModalImageContrast(value: number): Types.ManageImageActions {
  return {
    type: 'CHANGE_MODAL_IMAGE_CONTRAST',
    payload: value
  }
}

export function ChangeModalImageZoomLevel(value: number): Types.ManageImageActions {
  return {
    type: 'CHANGE_MODAL_IMAGE_ZOOM_LEVEL',
    payload: value
  }
}