import React, { PureComponent } from 'react';
import ReactTable, { Column, Filter } from "react-table";

import { IEasyTokensGetResponse, IEasyTokenExtendResponse, Path, IEasyTokenGenerateResponse } from 'interfaces/apis/easyToken';
import { POST, DELETE } from 'server';

import moment from 'moment';

interface IEasyTokenState {
  userEasyTokens: IEasyTokensGetResponse,
  isGeneratingNewToken: boolean
}

class EasyToken extends PureComponent<{}, IEasyTokenState> {

  state: IEasyTokenState = {
    userEasyTokens: [],
    isGeneratingNewToken: false
  }

  async componentDidMount() {
    let resp = await POST<any, IEasyTokensGetResponse>(Path.EasyTokenGet);

    this.setState({
      userEasyTokens: resp.data
    })
  }

  onExtendButtonClicked = async (tokenValue: string, userId?: number) => {
    if (confirm(`Are you sure to extend token ${tokenValue}${userId ? ` (User Id: ${userId})` : ''} token expire time?`)) {
      let newET = await POST<string, IEasyTokenExtendResponse>(Path.EasyTokenExtend, JSON.stringify(tokenValue));

      let updatedUserId = newET.data.uid;

      this.setState((prev) => {
        return {
          userEasyTokens: prev.userEasyTokens.map(et => {
            if (et.etv === tokenValue) {
              et = newET.data;
            }
            return et;
          })
        }
      })
    }
  }

  onRemoveButtonClicked = async (tokenValue: string, username?: string) => {
    if (confirm(`Are you sure to remove token ${tokenValue}${username ? ` (${username})` : ''}`)) {
      let resp = await DELETE(Path.EasyTokenRemove, JSON.stringify(tokenValue));
      if (resp.data) {
        this.setState((prev) => {
          return {
            userEasyTokens: prev.userEasyTokens.filter(et => {
              return et.etv !== tokenValue
            })
          }
        });
      }
    }
  }

  onAssignButtonClicked = (tokenValue: string) => {
    // Popup assign box
    alert(`Token ${tokenValue} assigned.`);
  }

  onGenerateNewTokenClicked = async () => {
    this.setState({
      isGeneratingNewToken: true
    });

    let resp = await POST<any, IEasyTokenGenerateResponse>(Path.EasyTokenGenerate);

    this.setState(prev => {
      return {
        userEasyTokens: [...prev.userEasyTokens, resp.data]
      }
    });

    this.setState({
      isGeneratingNewToken: false
    })
  }

  column: Column[] = [
  {
    Header: 'Username',
    filterable: true,
    filterMethod: (filter: Filter, rows: any[], column: any) => rows.un && rows.un.toLowerCase().indexOf(filter.value.toLowerCase()) >= 0,
    accessor: 'un',
    Cell: props => props.value || '-'
  },
  {
    Header: 'User Id',
    filterable: true,
    accessor: 'uid',
    Cell: props => props.value || '-'
  },
  {
    Header: 'Access Token',
    accessor: 'etv',
    filterable: true,
    filterMethod: (filter: Filter, rows: any[], column: any) => rows.etv && rows.etv.toLowerCase().indexOf(filter.value.toLowerCase()) >= 0,
    Cell: props => props.value || '-'
  },
  {
    Header: 'Expire',
    accessor: 'e',
    Cell: props => moment(props.value).format('DD-MMM-YYYY HH:mm:ss')
  },
  {
    id: 'action',
    Header: 'Action',
    accessor: a => a,
    sortable: false,
    Cell: props => {
      return (
        <div style={{ display: 'inline-block' }} className={'btn-group'} role={'group'}>
          <button onClick={() => this.onExtendButtonClicked(props.value.etv, props.value.uid)} className={'btn btn-success'}>Extend</button>
          <button onClick={() => this.onRemoveButtonClicked(props.value.etv, props.value.un)} className={'btn btn-danger'}>Remove</button>
        </div>
      )
    }
  }];

  render() {

    const { userEasyTokens, isGeneratingNewToken } = this.state;

    return (
      <div>
        <div style={{display: 'inline-block', width: '100%'}}>
          <h1 style={{float: 'left'}}>Manage Access Token</h1>
          <button style={{ float: 'right' }} disabled={isGeneratingNewToken} 
            className={'btn btn-success'} onClick={this.onGenerateNewTokenClicked}
          >
            {isGeneratingNewToken ? 'Generating...' : 'Generate New Token'}
          </button>
        </div>
        <ReactTable
          data={userEasyTokens}
          columns={this.column}
          defaultPageSize={10}
          showPageSizeOptions={false}
        />
      </div>
    );
  }
}

export default EasyToken;