import axios, { AxiosRequestConfig } from 'axios';
import Auth from 'components/Auth';

// Please add environment var switching
const endpoint: string = 'https://localhost:44320/api';
// const endpoint: string = 'http://104.248.150.50:81/api';

function onSessionExpired() {
  if (!!localStorage.getItem('token')) {
    alert('Your session has expired, please relogin again to continue.');
    Auth.resetAllToken();
    window.location.reload();
  }
}

async function GET<TResponse>(relUrl: string, config?: AxiosRequestConfig) {

  let accessToken = Auth.getAccessToken();

  return await axios.get<TResponse>(`${relUrl}`, {
    withCredentials: true,
    baseURL: endpoint,
    headers: {
      ...(accessToken && { Authorization: `Bearer ${accessToken}` }),
      "Content-Type": "application/json"
    },
    method: 'GET',
    ...config,
  }).catch(err => {
    if (axios.isCancel(err)) {
      console.warn('Request cancelled', err.message);
    }else{
      if (err.response && err.response.status !== 401)
        alert(err.response.data || err.response.data.message);
      else onSessionExpired();
    }

    throw err;
  });
}

async function POST<TRequest, TResponse>(relUrl: string, body?: TRequest, config?: AxiosRequestConfig) {

  let accessToken = Auth.getAccessToken();

  return await axios.post<TResponse>(`${relUrl}`, body, {
    withCredentials: true,
    baseURL: endpoint,
    headers: {
      ...(accessToken && { Authorization: `Bearer ${accessToken}` }),
      "Content-Type": "application/json"
    },
    method: 'POST',
    ...config,
  }).catch(err => {

    if (axios.isCancel(err)) {
      console.warn('Request cancelled', err.message);
    } else {
      if (err.response && err.response.status !== 401)
        alert(err.response.data || err.response.data.message);
      else onSessionExpired();
    }
      
    throw err;
  });
}

async function DELETE<TRequest>(relUrl: string, body?: TRequest, config?: AxiosRequestConfig) {

  let accessToken = Auth.getAccessToken();

  return await axios.delete(`${relUrl}`, {
    withCredentials: true,
    baseURL: endpoint,
    headers: {
      ...(accessToken && { Authorization: `Bearer ${accessToken}` }),
      "Content-Type": "application/json"
    },
    data: body,
    method: 'DELETE',
    ...config,
  }).catch(err => {

    if (axios.isCancel(err)) {
      console.warn('Request cancelled', err.message);
    } else {
      if (err.response && err.response.status !== 401)
        alert(err.response.data || err.response.data.message);
      else onSessionExpired();
    }

    throw err;
  });
}

export { GET, POST, DELETE }