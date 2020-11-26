import { IManageImageState, ManageImageActions } from './types';
import { IQuestionGroupType } from 'interfaces/apis/question';
import { string } from 'prop-types';

const defaultState: IManageImageState = {
  selectedTestId: undefined,
  testList: [],
  testSetList: [],
  testSetTitle: '',
  testSetQuestions: [],
  cells: [],
  selectedTestSetId: undefined,
  isModalOpened: false,
  onCreateNew: false,
  modal: {
    header: '',
    imageUrl: '',
    questions: [],
    brightness: 1,
    contrast: 1,
    zoomLevel: 2
  },
  isFetching: false,
  isCreateOrUpdating: false,
  formData: {
    tst: '',
    tid: undefined,
    tsq: [],
    c: []
  },
  hideRowsIndices: [-1]
}

export default (state: IManageImageState = defaultState, action: ManageImageActions) => {

  switch (action.type){
    case 'GET_TEST_SET_LIST':
    state = {
      ...defaultState,
      selectedTestId: state.selectedTestId,
      testList: state.testList,
      testSetList: action.payload,
    }
    break;

    case 'GET_TEST_LIST':
      state = {
        ...state,
        testList: action.payload,
      }
    break;

    case 'GET_TEST_SET_QUESTION':
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
          tst: action.payload.tst,
          tid: state.selectedTestId,
          c: action.payload.c.map(c => {
            return {
              cid: c.cid,
              cimg: c.cimg,
              acpjson: c.acpjson,
              forceRefreshId: Date.now()
            }
          }),
          tsq: action.payload.tsq.map((q, i) => {
            return {
              tqid: q.tqid,
              cid: q.cid,
              qid: q.qid,
              aid: q.a.filter(a => a.ica).map(a => a.aid)
            }
          })
        },
        onCreateNew: !action.payload.tst
      }
      break;

      case 'ON_TEST_TITLE_CHANGED':
      state = {
        ...state,
        testSetTitle: action.payload,
        formData: { 
          ...state.formData,
          tst: action.payload
        },
        testSetList: state.testSetList.map(ts => {
          if(ts.tsid === state.selectedTestSetId)
            ts.tst = action.payload;
            
          return ts;
        })
      }
      break;

      case 'ON_IMAGE_URL_CHANGED':
      case 'ON_IMAGE_ID_CHANGED':
      state = {
        ...state,
        formData: {
          ...state.formData,
          c: action.payload
        }
      }
      break;

    case 'RESET_ON_FETCH_TEST_QUESTION':
      state = {
        ...defaultState,
        testList: state.testList,
        selectedTestId: state.selectedTestId,
        testSetList: state.testSetList,
        formData: defaultState.formData
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

    case 'HIDE_ROW_INDICES':
    state = {
      ...state,
      hideRowsIndices: action.payload
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

    case 'SET_SELECTED_TEST_SET_ID':
      state = {
        ...state,
        selectedTestSetId: action.payload
      }
      break;

    case 'TOGGLE_TEST_QUESTION_IS_FETCHING':
      state = {
        ...state,
        isFetching: action.payload
      }
      break;

    case 'TOGGLE_TEST_QUESTION_IS_CREATE_OR_UPDATING':
      state = {
        ...state,
        isCreateOrUpdating: action.payload
      }
      break;

      case 'ON_ANSWER_SELECTED':
      state = {
        ...state,
        formData: action.payload
      }
      break;

      case 'ON_TEST_QUESTION_DELETED':
      state = {
        ...state,
        testSetList: state.testSetList.filter(x => x.tsid !== action.payload)
      }
      break;

      case 'SET_SELECTED_TEST_ID':
      state = {
        ...state,
        selectedTestId: action.payload
      }
      break;

  }
  return state;
}