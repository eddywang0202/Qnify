import React, { PureComponent, ComponentElement, ComponentType } from 'react';
import { Redirect } from 'react-router-dom';
import * as jwtDecode from 'jwt-decode';

import { POST } from 'server';

import { IAuth, IJwtModel, IRefreshJwtModel, IUserIdentity, IRole } from './types';
import { Path } from 'interfaces/apis/easyToken';
import { dispatch } from '../../configureStore';

class Auth {

  setAccessToken = (token) => {
    localStorage.setItem('token', token)
  }

  getAccessToken = () => {
    return localStorage.getItem('token')
  }

  setRefreshToken = (token) => {
    localStorage.setItem('refresh-token', token)
  }

  resetAllToken = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refresh-token');
    dispatch({
      type: 'LOGOUT'
    });
  }

  authenticateEzToken = async function(userInputToken: string) {
    try {
      let resp = await POST<string, string>(Path.EasyTokenVerify, JSON.stringify(userInputToken));
      this.setAccessToken(resp.data);
      this.setIdentity(resp.data);
      return true;
    } catch (err) {

      if (err.response.status === 401)
        return false;

      throw err;
    };
  }

authenticate = async function(username: string, password: string) {

  // Do not revalidate if already logged in.
  if (this.isAuthenticated) return this.isAuthenticated;

  // Verify to server
  try {
    let resp = await POST<any, any>('/auth', {
      u: username,
      p: password
    });

    if (resp.data != null) {
      this.setAccessToken(resp.data.accessToken);
      this.setIdentity(resp.data.accessToken);
      return true;
    }
  }
  catch (err) {
    if (err.response.status === 401) return false;
    else throw err;
  }

  return false;
}

get isAuthenticated() {
  let token = localStorage.getItem('token');
  let refreshToken = localStorage.getItem('refresh-token');

  if (token) {
    try {
      const dToken = jwtDecode<IJwtModel>(token);
      let expireTime = dToken.exp;

      if (expireTime && expireTime > (new Date().getTime() / 1000)) {
        return true;
      } else {
        if (refreshToken) {
          const dRefreshToken = jwtDecode<IRefreshJwtModel>(refreshToken);

          if (dRefreshToken.exp > (new Date().getTime() / 1000)) {
            //todo: request with refresh token
            return true;
          }
        }
      }
    }
    catch (err) {
      this.resetAllToken();
      return false;
    }
  }
  return false;
}

  setIdentity = (token: string) : IUserIdentity =>  {
    const dToken = jwtDecode<IJwtModel>(token);

    return {
      ...this.getIdentity(),
      name: dToken.name,
      role: dToken.role
    }
  }

  getIdentity = () : IUserIdentity => {
    let token = localStorage.getItem('token');

    if (token) {
      const dToken = jwtDecode<IJwtModel>(token);
      let identity: IUserIdentity = {
        name: dToken.name,
        role: dToken.role
      }

      return identity;
    }

    return {
      role: null
    }
  }
}

const auth = new Auth();

const Authorize = (allowedRoles: IRole[]) => (WrappedComponent) => {
  return class Authorize extends React.PureComponent {
    render() {
      const { role } = auth.getIdentity();

      if (!allowedRoles.includes(role)) {
        return <Redirect to={'/noaccess'} />
      } else if (!auth.isAuthenticated) {
        auth.resetAllToken();
        return <Redirect to={'/'} />
      } else {
        return <WrappedComponent {...this.props} />
      }
    }
  }
}

export default auth;
export { Authorize as AllowedRoles };