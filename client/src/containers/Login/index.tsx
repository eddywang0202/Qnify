import React, { PureComponent } from 'react';

import Auth from '../../components/Auth';
import { Redirect } from 'react-router-dom';
import { ILoginProp, ILoginState } from './types';

class Login extends PureComponent<ILoginProp, ILoginState> {

  state: ILoginState = {
    authorized: false,
    username: '',
    password: '',
    isLoggingIn: false,
    errorMessage: ''
  }

  componentDidMount() {
    this.setState({
      authorized: Auth.isAuthenticated
    });
  }

  onLoginFormSubmit = async (e: React.FormEvent<HTMLFormElement>) => {

    this.setState({
      isLoggingIn: true
    });

    e.preventDefault();

    let username = e.target[0].value;
    let password = e.target[1].value;

    try{
      let authed = await Auth.authenticate(username, password);

      if (authed){
        this.setState({
          authorized: authed
        });
      }
      else
      {
        this.setState({
          errorMessage: 'Invalid username or password.',
          isLoggingIn: false
        });
      }
    }
    catch(err) {
      this.setState({
        isLoggingIn: false,
        errorMessage: 'There is problem while requesting login to the server, please try again.'
      });
    }
  }


  render() {

    const { authorized, isLoggingIn, errorMessage } = this.state;

    if (authorized) {
      return <Redirect to={'/'} />
    }

    return (
      <div className={'container h-100'}>
        <div className='row justify-content-center align-items-center h-100'>
          <div className="card">
            <div className="card-header">
              <h5>Admin Login</h5>
            </div>
            <div className="card-body">
              <form onSubmit={this.onLoginFormSubmit}>
                <div className="form-group">
                  <label htmlFor="username">Username</label>
                  <input type="textbox" maxLength={20} className="form-control" id="username" />
                </div>
                <div className="form-group">
                  <label htmlFor="password">Password</label>
                  <input type="password" maxLength={32} className="form-control" id="password" />
                </div>
                <div className="form-group">
                  {/* <span className='msg-no-access'>Wrong username or password</span> */}
                </div>
                {errorMessage && <p style={{ color: 'rgb(255, 50, 50)', margin: '0 auto', width: 250 }}>{errorMessage}</p>}
                <br/>
                <button disabled={isLoggingIn} type='submit' className='btn btn-primary btn-block'>{isLoggingIn ? 'Logging In...' : 'Log in'}</button>
              </form>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Login;