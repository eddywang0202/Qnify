import React, { PureComponent } from 'react';

import { withRouter } from 'react-router-dom';
import Auth from '../../components/Auth';
import { GET } from 'server';
import { IGetTestInfoResponse, Path } from 'interfaces/apis/test';

interface IHomeState {
  testTitle: string,
  enableAccess: boolean,
  invalidTokenMessage: string,
  isSubmitting: boolean,
  isLoadingPage: boolean
}

class Home extends PureComponent<any, IHomeState> {

  state = {
    testTitle: '',
    enableAccess: false,
    invalidTokenMessage: '',
    isSubmitting: false,
    isLoadingPage: true
  }

  handleKeyPress(e, exclusion: number[] = null) {

    let inp = String.fromCharCode(e.which || e.charCode || e.keyCode || 0);

    if (!/^[a-zA-Z0-9]*$/.test(inp) && !(exclusion && exclusion.includes(e.which || e.charCode || e.keyCode))) {
      e.preventDefault();
    }
  }

  handleKeyUp(e) {

    e.target.value = e.target.value.toUpperCase();

    e.preventDefault();

    if ((e.keyCode >= 48 && e.keyCode <= 57) 
      || e.keyCode >= 65 && e.keyCode <= 90) {

      let next = e.target.nextElementSibling;

      if (next && next.tagName === "INPUT") {
        next.focus();
        next.select();
      }
    }
  }

  async componentDidMount() {
    const testInfoResponse = await GET<IGetTestInfoResponse>(Path.GetActiveTestInfo);
    const testInfo = testInfoResponse.data;

    await this.setState({
      enableAccess: testInfo.s,
      testTitle: testInfo.t,
      isLoadingPage: false
    })
  }

  onFormSubmit = async (e) => {
    e.preventDefault();

    this.setState({
      invalidTokenMessage: '',
      isSubmitting: true,
    });

    let formValue = e.target;

    let first   = formValue[0].value;
    let second  = formValue[1].value;
    let third   = formValue[2].value;
    let forth   = formValue[3].value;
    let fifth   = formValue[4].value;

    let keyedToken: string = first + second + third + forth + fifth;

    if (!keyedToken || keyedToken.length != 5) {
      this.setState({
        invalidTokenMessage: 'Token cannot be empty and must be filled.',
        isSubmitting: false
      });

      return;
    }

    // Validate token here
    try{
      if (await Auth.authenticateEzToken(keyedToken)) {
        if (!Auth.getIdentity().name)
          this.props.history.push(`/start?etk=${keyedToken}`);
        else
          this.props.history.push(`/`);
      }
      else
      {
        this.setState({
          invalidTokenMessage: 'You\'ve entered an invalid token, please try again.'
        });
      }
    }
    catch(err) {
      this.setState({
        invalidTokenMessage: 'There is problem while requesting login to the server, please try again.'
      });
    }

    this.setState({
      isSubmitting: false
    })
  }

  render() {

    const { invalidTokenMessage, isSubmitting, testTitle, enableAccess, isLoadingPage } = this.state;

    if (isLoadingPage) return null;

    return (
      <div className={'home container h-100'}>
        <div className='row justify-content-center align-items-center h-100'>
          <div className="card">
            <div className="card-header">
              <h2>{testTitle}</h2>
            </div>
            <div className="card-body">
              {
                enableAccess
              ?
              <div>
                <h5 className="card-title">Please enter your access token down below to continue</h5>
                <br/>
                {invalidTokenMessage && <p style={{ color: 'rgb(255, 150, 122)', margin: '0 auto' }}>{invalidTokenMessage}</p>}
                <form className='form-inline' onSubmit={this.onFormSubmit}>
                  <div className='input-blocks'>
                    <input type="text" onKeyPress={this.handleKeyPress} onKeyUp={this.handleKeyUp} className="form-control mr-sm-2" maxLength={1}/>
                    <input type="text" onKeyPress={this.handleKeyPress} onKeyUp={this.handleKeyUp} className="form-control mr-sm-2" maxLength={1}/>
                    <input type="text" onKeyPress={this.handleKeyPress} onKeyUp={this.handleKeyUp} className="form-control mr-sm-2" maxLength={1}/>
                    <input type="text" onKeyPress={this.handleKeyPress} onKeyUp={this.handleKeyUp} className="form-control mr-sm-2" maxLength={1}/>
                    <input type="text" onKeyPress={(e) => this.handleKeyPress(e, [13])} onKeyUp={this.handleKeyUp} className="form-control mr-sm-2" maxLength={1}/>
                  </div>
                  <button disabled={isSubmitting} className="btn btn-lg btn-block btn-primary">{isSubmitting ? 'Verifying...': 'Enter'}</button>
                </form>
              </div>
              :
              <div>Test entry is not opened yet, please come back later.</div>
              }
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default withRouter(Home);