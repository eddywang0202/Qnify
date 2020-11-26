import { ITestPageState, TestQuestionActions } from "./types";
import { findLastIndex } from "utils/array";

const defaultState: ITestPageState = {
  testSetList: [],
  testSetQuestions: [],
  cells: [],
  isFetching: false,
  testSetTitle: '',
  selectedTestSetId: 0,
  isModalOpened: false,
  modal: {
    header: '',
    questions: [],
    imageUrl: '',
    brightness: 1,
    contrast: 1,
    zoomLevel: 2,
    info: ['[{}]']
  },
  formData: {
    uqas: []
  },
  isFormSubmitting: false,
  hideRowsIndices: [-1]
}

export default (state: ITestPageState = defaultState, action: TestQuestionActions) => {

  switch(action.type) {
    case 'ON_GET_QUESTION_LIST':

      let lastUnansweredTestSet = action.payload[findLastIndex(action.payload, 'ia', true) + 1];

    state = {
      ...state,
      testSetList: action.payload,
      selectedTestSetId: lastUnansweredTestSet && lastUnansweredTestSet.tsid
    }
    break;

    case 'GET_TEST_QUESTION':
      state = {
        ...state,
        testSetTitle: action.payload.tst,
        testSetQuestions: action.payload.tsq.map((q, i) => {
          return {
            ...q,
            tqid: (!!q.tqid) ? q.tqid : i
          }
        }),
        cells: action.payload.c,
        selectedTestSetId: action.payload.tsid,
        formData: {
          uqas: action.payload.tsq.map((q, i) => {
            return {
              tqid: q.tqid,
              aid: q.a.filter(a => a.ica).map(a => a.aid)
            }
          })
        },
      }
      break;

    case 'ON_QUESTION_UPDATED_SELECTED':
      state = {
        ...state,
        testSetQuestions: action.payload
      }
      break;

    case 'SET_SELECTED_TEST_SET_ID':
      state = {
        ...state,
        selectedTestSetId: action.payload
      }
    break;

    case 'HIDE_ROW_INDICES':
    state = {
      ...state,
      hideRowsIndices: action.payload
    }
    break;

    case 'SET_MODAL':
      state = {
        ...state,
        isModalOpened: true,
        modal: {
          ...state.modal,
          ...action.payload
        }
      }
      break;

    case 'RESET_MODAL':
      state = {
        ...state,
        isModalOpened: false,
        modal: {
          ...defaultState.modal
        }
      }
      break;

    case 'RESET_MODAL_ADJUSTMENT':
      state = {
        ...state,
        modal: {
          ...state.modal,
          brightness: defaultState.modal.brightness,
          contrast: defaultState.modal.contrast,
          zoomLevel: defaultState.modal.zoomLevel,
        }
      }
      break;

    case 'CHANGE_MODAL_IMAGE_BRIGHTNESS':
      state = {
        ...state,
        modal: {
          ...state.modal,
          brightness: action.payload
        }
      }
      break;

    case 'CHANGE_MODAL_IMAGE_CONTRAST':
      state = {
        ...state,
        modal: {
          ...state.modal,
          contrast: action.payload
        }
      }
      break;

    case 'CHANGE_MODAL_IMAGE_ZOOM_LEVEL':
      state = {
        ...state,
        modal: {
          ...state.modal,
          zoomLevel: action.payload
        }
      }
      break;

    case 'ON_ANSWER_SELECTED':
      state = {
        ...state,
        formData: action.payload
      }
      break;

      case 'ON_FORM_SUBMIT':
      let selectedTestSetIndex = state.testSetList.findIndex(x => x.tsid === action.payload);
      state.testSetList[selectedTestSetIndex].ia = true;
      break;

      case 'TOGGLE_TEST_QUESTION_IS_FETCHING':
      state = {
        ...state,
        isFetching: action.payload
      }
      break;

      case 'TOGGLE_TEST_QUESTION_IS_SUBMITTING':
      state = {
        ...state,
        isFormSubmitting: action.payload
      }
      break;

    case 'RESET_ON_FETCH_TEST_QUESTION':
      state = {
        ...defaultState,
        testSetList: state.testSetList,
        selectedTestSetId: state.selectedTestSetId,
        formData: defaultState.formData
      }
      break;
  }

  return state;
}