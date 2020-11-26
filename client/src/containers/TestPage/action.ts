import * as Types from './types';
import { IImageModal } from '../ManageImage/types';
import { Dispatch } from 'redux';
import { ITestQuestionResponse, Path, IGetTestQuestionListResponse, IUpdateParticipantTestQuestionRequest, IQuestion } from 'interfaces/apis/question';
import { GET, POST } from 'server';
import { IControlSelectionValue } from '~/components/Controls/types';

export function OnFormSubmit(testSetId: number, formData: IUpdateParticipantTestQuestionRequest) {
  return async (dispatch: Dispatch<Types.TestQuestionActions>) => {

    let resp = await POST<IUpdateParticipantTestQuestionRequest, boolean>(Path.UpdateParticipantTestQuestion, formData);

    if (resp.data) {
      dispatch({
        type: 'ON_FORM_SUBMIT',
        payload: testSetId
      });
    }
  }
}

export function OnAnswerSelected(selected: IControlSelectionValue) {
  return (dispatch: Dispatch<Types.TestQuestionActions>, getState: () => { testPage: Types.ITestPageState }) => {

    let state = getState().testPage;

    var newFormData: IUpdateParticipantTestQuestionRequest = {
      ...state.formData,
      uqas: state.formData.uqas.map(d => {
        if (d.tqid === selected.testQuestionId)
          return {
            tqid: selected.testQuestionId,
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

    var stateQuestion : IQuestion[] = state.testSetQuestions;
    let newQuestions = stateQuestion.map(q => {
      if (q.tqid === selected.testQuestionId)
        q.a.map(a => {
          if (Number(selected.answerId) === a.aid) a.ica = !a.ica;
          return a;
        })
      return q;
    });

    dispatch({
      type: 'ON_QUESTION_UPDATED_SELECTED',
      payload: newQuestions
    });
  }
}

export function GetTestSetQuestionList() {
  return async (dispatch: Dispatch<Types.TestQuestionActions>) => {

    let resp = await GET<IGetTestQuestionListResponse[]>(Path.GetActiveTestQuestionList);

    dispatch({
      type: 'ON_GET_QUESTION_LIST',
      payload: resp.data
    });
  }
}

export function GetTestSetQuestionsById(testQuestionId: number) {
  return async (dispatch: Dispatch<Types.TestQuestionActions>) => {

    let resp = await GET<ITestQuestionResponse>(Path.GetParticipantTestQuestion(testQuestionId));

    dispatch({
      type: 'GET_TEST_QUESTION',
      payload: resp.data
    });
  }
}

export function HideRowIndices(hideRowsIndices: number[]): Types.TestQuestionActions {
  return {
    type: 'HIDE_ROW_INDICES',
    payload: hideRowsIndices
  }
}

export function SetSelectedTestSetId(testSetId: number): Types.TestQuestionActions {
  return {
    type: 'SET_SELECTED_TEST_SET_ID',
    payload: testSetId
  }
}

export function ResetOnFetchTestQuestion(): Types.TestQuestionActions {
  return {
    type: 'RESET_ON_FETCH_TEST_QUESTION',
  }
}

export function ToggleIsFetching(isFetching: boolean): Types.TestQuestionActions {
  return {
    type: 'TOGGLE_TEST_QUESTION_IS_FETCHING',
    payload: isFetching
  }
}

export function ToggleIsSubmitting(isSubmitting: boolean): Types.TestQuestionActions {
  return {
    type: 'TOGGLE_TEST_QUESTION_IS_SUBMITTING',
    payload: isSubmitting
  }
}

export function SetModal(modal: IImageModal): Types.TestQuestionActions {
  return {
    type: 'SET_MODAL',
    payload: modal
  }
}

export function ResetModal(): Types.TestQuestionActions {
  return {
    type: 'RESET_MODAL',
  }
}

export function ResetModalImageAdjustment(): Types.TestQuestionActions {
  return {
    type: 'RESET_MODAL_ADJUSTMENT',
  }
}

export function ChangeModalImageBrightness(value: number): Types.TestQuestionActions {
  return {
    type: 'CHANGE_MODAL_IMAGE_BRIGHTNESS',
    payload: value
  }
}

export function ChangeModalImageContrast(value: number): Types.TestQuestionActions {
  return {
    type: 'CHANGE_MODAL_IMAGE_CONTRAST',
    payload: value
  }
}

export function ChangeModalImageZoomLevel(value: number): Types.TestQuestionActions {
  return {
    type: 'CHANGE_MODAL_IMAGE_ZOOM_LEVEL',
    payload: value
  }
}