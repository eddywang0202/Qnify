import * as React from 'react';
import { Switch, Route, withRouter, RouteComponentProps } from 'react-router-dom';

import App from './App';
import Login from './containers/Login';
import Home from './containers/Home';

import Register from './containers/Register';
import TestPage from './containers/TestPage';
import TestResult from './containers/TestResult';

import AdminPanel from './containers/AdminPanel';
import EasyToken from './containers/EasyToken';
import ManageImage from './containers/ManageImage';
import ConsentPage from './containers/ConsentPage';
import AdminReportPage from './containers/AdminReportPage';
import ManageTest from './containers/ManageTest';

import Auth, { AllowedRoles } from './components/Auth';
import { IRole, IUserIdentity } from './components/Auth/types';
import NoAccess from './containers/NoAccess';
import Sidebar from './components/Sidebar';
import { StaticContext, Redirect } from 'react-router';

const NotFound = () => (
  <div style={{width: '100%', height: '100%', backgroundColor: 'white', color: 'black'}}>
    Not Found
  </div>
)

const Guest = AllowedRoles(['guest']);
const Member = AllowedRoles(['member']);
const Admin = AllowedRoles(['admin']);

interface IRouter {
  role: IRole
}

// class Routes extends React.PureComponent<{}, IRouter>{
class Routes extends React.PureComponent{

  renderRootPage = (props: RouteComponentProps<{}, StaticContext, {}>, identity: IUserIdentity) : React.ReactNode => {
    if (identity.role === 'admin')
      return <Redirect to='/admin' />

    if (identity.role === 'member')
    {
      return <Redirect to='/test' />
    }

    Auth.resetAllToken();
    return <Home />
  }

  render() {

    const userIdentity = Auth.getIdentity();
    const { role } = userIdentity;

    const showLobbyBackground: boolean = !role;

    return (
      <App className={`${role === 'admin' ? 'admin-app' : 'app'}${showLobbyBackground ? ' bg' : ''}`}>
        {!showLobbyBackground && <Sidebar userIdentity={userIdentity}/>}
        <div className={showLobbyBackground ? 'inline' : 'main'}>
          <Switch>
            <Route path="/" exact={true} render={(props) => this.renderRootPage(props, userIdentity)} />
            <Route path="/login" exact={true} render={(props) => <Login {...props} />} />,
            
            { /* Admin Routes */ }
            <Route path="/admin" exact={true} component={Admin(AdminPanel)} />
            <Route path="/token" exact={true} component={Admin(EasyToken)} />
            <Route path="/image" exact={true} component={Admin(ManageImage)} />
            <Route path="/consent" exact={true} component={Admin(ConsentPage)} />
            <Route path="/report" exact={true} component={Admin(AdminReportPage)} />
            <Route path="/managetest" exact={true} component={Admin(ManageTest)} />

            { /* Member Routes */}
            <Route path="/start" exact={true} component={Guest(Register)} />
            <Route path="/test" exact={true} component={Member(TestPage)} />
            <Route path="/result" exact={true} component={Member(TestResult)} />
            {/* <Route path="/:id" component={Member(Test)} /> */}
          
            <Route path="/noaccess" component={NoAccess} />
            <Route path="/" component={NotFound} />
          </Switch>
        </div>
      </App>
    );
  }
};

export default withRouter(Routes);