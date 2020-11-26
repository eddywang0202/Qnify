import { applyMiddleware, createStore, combineReducers, compose } from 'redux';
import { createBrowserHistory } from 'history';
import { connectRouter, routerMiddleware } from 'connected-react-router';
import dashboard from './containers/Dashboard/reducer';
import manageImage from './containers/ManageImage/reducer';
import testPage from './containers/TestPage/reducer';
import thunk from 'redux-thunk';

const history = createBrowserHistory();

const createRootReducer = (history) => combineReducers({
  router: connectRouter(history),
  dashboard,
  manageImage,
  testPage
});

const appReducer = history => (state, action) => {

  if(action.type === 'LOGOUT') {
    state = undefined;
  }

  return createRootReducer(history)(state, action);
}

const store = createStore(
  appReducer(history),
  compose(
    applyMiddleware(
      routerMiddleware(history),
      thunk
    )
  )
);

export const dispatch = store.dispatch;
export { store, history };