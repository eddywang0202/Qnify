import React, { PureComponent } from 'react';
import ReactTable, { Column, Filter } from "react-table";
import { Path, IGetParticipantDetailReportResponse, IGetParticipantReportResponse, IGetParticipantReportRequest, IParticipantReportList, IGetParticipantDetailReportRequest } from 'interfaces/apis/report';
import { Path as TestPath } from 'interfaces/apis/test';

import moment from 'moment';
import { GET, POST } from 'server';

import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { QuestionType } from 'interfaces/apis/question';
import { IGetTestInfoResponse } from 'interfaces/apis/test';
import { Path as UserPath,  GetUserDemographicInfoResponse } from 'interfaces/apis/user';

interface IDetailInfo {
  testSetName?: string,
  correctAnswer?: number,
  questionCount?: number,
  username?: string,
  testResult?: IGetParticipantDetailReportResponse,
  demographicResult?: GetUserDemographicInfoResponse
}

interface IAdminReportPageState {
  userTestResults: IParticipantReportList[],
  take: number,
  offset: number,
  currentPageIndex: number,
  totalPageCount: number,
  isModalOpened: boolean,
  detailInfo: IDetailInfo,
  isFetching: boolean,
  filterUsername: string,
  filterTestList: IGetTestInfoResponse[],
  filterTestId: number
}

class AdminReportPage extends PureComponent<any, IAdminReportPageState> {

  state: IAdminReportPageState = {
    userTestResults: [],
    take: 10,
    offset: 0,
    currentPageIndex: 0,
    totalPageCount: 3,
    isModalOpened: false,
    detailInfo: {},
    isFetching: false,
    filterTestList: [],
    filterTestId: undefined,
    filterUsername: undefined
  }

  fetchParticipantDetailedReport = async (testId: number, userId: number, username: string, testSetName: string, correctAnswerCount: number, questionCount: number) => {
    let resp = await POST<IGetParticipantDetailReportRequest, IGetParticipantDetailReportResponse>(Path.GetParticipantDetailReport(testId, userId));
    this.setState(prevState => {
      return {
        ...prevState,
        detailInfo: {
          ...prevState.detailInfo,
          testResult: resp.data,
          testSetName,
          correctAnswer: correctAnswerCount,
          questionCount,
          username
        }
      }
    });
  }

  fetchDemographicProfile = async(testId: number, userId: number) => {
    let resp = await GET<GetUserDemographicInfoResponse>(UserPath.GetUserDemographicInfo(testId, userId));
    this.setState(prevState => {
      return {
        ...prevState,
        detailInfo: {
          ...prevState.detailInfo,
          demographicResult: resp.data
        }
      }
    });
  }

  onDetailButtonClicked = async (testId: number, userId: number, username: string, testSetName: string, correctAnswerCount: number, questionCount: number) => {

    await Promise.all([
      this.fetchParticipantDetailedReport(testId, userId, username, testSetName, correctAnswerCount, questionCount),
      this.fetchDemographicProfile(testId, userId)
    ]);

    this.setState({
      isModalOpened: true
    })
    // Get detailed report here
  }

  onPageSizeChanged = async (newPageSize: number, newPage: number) => {
    await this.setState((prevState) => {
      return {
        ...prevState,
        offset: (newPage) * newPageSize,
        take: newPageSize,
        currentPageIndex: newPage
      }
    })

    await this.fetchData();
  }

  onPageChanged = async (newPageIndex: number) => {;
    const { take } = this.state;

    let newOffset = (newPageIndex) * take;

    await this.setState((prevState) => {
      return {
        ...prevState,
        offset: newOffset,
        currentPageIndex: newPageIndex
      }
    });

    await this.fetchData();
  }

  fetchData = async () => {

    const { userTestResults, currentPageIndex, take, offset, filterUsername, filterTestId } = this.state;

    if (!!filterUsername || !!filterTestId) {
      userTestResults.length = 0;
    }

    this.setState(prevState => {
      return {
        ...prevState,
        isFetching: true,
      }
    });

    try {
      // Call API get user data
      const resp = await POST<IGetParticipantReportRequest, IGetParticipantReportResponse>(Path.GetParticipantReport, {
        l: take,
        o: offset,
        un: filterUsername,
        tid: filterTestId
      });

      this.setState((prevState) => {
        return {
          ...prevState,
          totalPageCount: resp.data.tpc,
          userTestResults: resp.data.utqr,
        }
      });
    } catch (err) {
      console.error(err);
    } finally {
      this.setState((prevState) => {
        return {
          ...prevState,
          isFetching: false
        }
      });
    }
  }

  fetchTestList = async () => {
    let resp = await GET<IGetTestInfoResponse[]>(TestPath.GetAllTestInfo)
    this.setState(prevState => {
      return {
        ...prevState,
        filterTestList: resp.data
      }
    });
  }

  async componentDidMount() {
    await Promise.all([
      this.fetchData(),
      this.fetchTestList()
    ]);
  }

  filterUsernameTimeout : number = undefined;
  onFilterUsername = async (e: React.ChangeEvent<HTMLInputElement>) => {
    
    clearTimeout(this.filterUsernameTimeout);
    let filterUsername = e.target.value;

    await this.setState({
      offset: 0,
      currentPageIndex: 0,
      filterUsername
    })

    this.filterUsernameTimeout = setTimeout(async () => {
      if (!!filterUsername) await this.fetchData();
      else await this.fetchData();
    }, 1000);
  }

  onFilterTest = async (e: React.ChangeEvent<HTMLSelectElement>) => {
    await this.setState({
      offset: 0,
      currentPageIndex: 0,
      filterTestId: Number(e.target.value)
    });

    this.fetchData();
  }

  column: Column<IParticipantReportList>[] = [
    {
      Header: 'Username',
      id: 'username',
      accessor: (row) => row.un,
      sortable: false,
      Cell: props => props.value || '-'
    },
    {
      Header: 'Test Name',
      id: 'testName',
      accessor: (row) => row.tn,
      sortable: false,
      Cell: props => props.value || '-'
    },
    {
      Header: 'Submit Date Time',
      id: 'submitDateTime',
      accessor: (row) => row.sd,
      sortable: false,
      Cell: props => (!!props.value) ? moment(props.value).format('DD-MMM-YYYY HH:mm:ss') : '-'
    },
    {
      Header: 'Correct Answers / Question',
      id: 'caq',
      sortable: false,
      Cell: props => `${props.original.tcq} / ${props.original.tq}`
    },
    {
      id: 'action',
      Header: 'Action',
      accessor: (row) => row,
      sortable: false,
      Cell: props => {
        return (
          <div style={{ display: 'inline-block' }} className={'btn-group'} role={'group'}>
            <button onClick={() => this.onDetailButtonClicked(props.value.tid, props.value.uid, props.value.un, props.value.tn, props.value.tcq, props.value.tq)} className={'btn btn-link'}>Show Test Detail</button>
          </div>
        )
      }
    }
    ];

  render() {

    const { userTestResults, isModalOpened, take, totalPageCount, currentPageIndex, isFetching, detailInfo, filterTestList, filterTestId } = this.state;

    return (
      <div>
        <div style={{ display: 'inline-block', width: '100%' }}>
          <h1 style={{ float: 'left' }}>Participant's Test Result</h1>
        </div>
        <div className={'search-tool'}>
          <label>
            Username
            <input type='text' className={'form-control'} onChange={this.onFilterUsername} />
          </label>
          <label>
            Test
            <select className={'form-control'} onChange={this.onFilterTest} value={filterTestId}>
              <option value={undefined}>All</option>
              {
                filterTestList.map(t => {
                  return <option key={t.tid} value={t.tid}>{t.t}</option>
                })
              }
            </select>
          </label>
        </div>
        
        <ReactTable
          data={userTestResults}
          columns={this.column}
          defaultPageSize={take}
          loading={isFetching}
          pages={totalPageCount}
          page={currentPageIndex}
          loadingText={'Loading data from server, Please wait...'}
          manual
          showPageSizeOptions={true}
          onPageSizeChange={(newPageSize, newPage) => this.onPageSizeChanged(newPageSize, newPage)}
          onPageChange={(page) => this.onPageChanged(page)}
        />
        <Modal 
          isOpen={isModalOpened}
          className={'modal-image'}
          toggle={() => this.setState(prevState => { return { isModalOpened: !prevState.isModalOpened } })}
        >
          <ModalHeader>Test Report for {detailInfo.username}</ModalHeader>
          <ModalBody>
            <div>
              <div>
                <h5>Demographic Profile</h5>
                <div className={'card'}>
                  <div className={'card-body'}>
                    {
                      !!detailInfo.demographicResult
                      &&
                      detailInfo.demographicResult.tsq.length > 0
                      ?
                      detailInfo.demographicResult.tsq.map((dr, i) => {
                        return (
                          <div key={dr.qt} style={{ paddingBottom: 12 }}>
                            <div>
                              <p><strong>{dr.qt}</strong></p>
                              <p style={{ lineHeight: 1 }}> { dr.a.length > 0 ? dr.a.join(', ') : '-' }</p>
                            </div>
                          </div>
                        )
                      })
                      :
                      <p>No profile data filled in for this participant</p>
                    }
                  </div>
                </div>
              </div>
              <hr/>
              <h5>Test Result</h5>
              <table className={'table table-bordered'}>
                <thead className={"thead-dark"}>
                  <tr>
                    <th>Total Test Set Count</th>
                    <th>Total Question Count</th>
                    <th>Total Correct Answers Count</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td>{!!detailInfo.testResult ? detailInfo.testResult.rsm.length : 0}</td>
                    <td>{detailInfo.questionCount}</td>
                    <td>{detailInfo.correctAnswer}</td>
                  </tr>
                </tbody>
              </table>
              <hr />
              <h5>Test Performance</h5>
              {
                !!detailInfo.testResult
                &&
                <table className={'table table-bordered'}>
                  <thead className={"thead-dark"}>
                    <tr>
                      <th>Category</th>
                      <th>Rate</th>
                    </tr>
                  </thead>
                  <tbody>
                    {
                      Object.keys(detailInfo.testResult.rp).map((p, i) => {
                        let rate = detailInfo.testResult.rp[p].rpr;
                        let name = detailInfo.testResult.rp[p].rpn;
                        return (
                          <tr>
                            <td key={i}>{name}</td>
                            <td key={i}>{rate}</td>
                          </tr>
                        )
                      })
                    }
                  </tbody>
                </table>
              }
              <hr />
              <h5>Test Cases and Evaluation Breakdown</h5>
              {
                detailInfo.testResult && detailInfo.testResult.rsm.map((ts, tsIndex) => {
                  return (
                    <div key={tsIndex} style={{ paddingBottom: 12, paddingTop: 6 }}>
                      <div>
                        <p><span style={{ fontWeight: 'bold', fontSize: 32 }}>#{++tsIndex}</span> Test Set Title: <strong>{ts.tst}</strong></p>
                        <p>No. of Test Case Questions: <strong>{ts.tsq.length}</strong>, including optional question: <strong>{ts.tsq.filter(tq => tq.qtid === QuestionType.RadioButtonsOptional).length}</strong></p>
                      </div>
                      <div className={'card'}>
                        <div className={'card-body'}>
                          {
                            ts.tsq.map((tq, i) => {
                              let cellInfo = !!tq.cid && ts.c.find(cell => cell.cid === tq.cid);
                              let isOptionalQuestion = tq.qtid === QuestionType.RadioButtonsOptional;
                              return (
                                <div key={i}>
                                  <p><strong>{++i}. {tq.qt}</strong> {isOptionalQuestion && '(optional)'}</p>
                                  {
                                    !!cellInfo && 
                                    <div style={{fontSize: 10}}>
                                      <p style={{ lineHeight: 1 }}>Image ID: <strong>{cellInfo.acpjson}</strong></p>
                                      <p style={{ lineHeight: 1 }}>Image URL: <strong>{cellInfo.cimg}</strong></p>
                                      <p style={{ lineHeight: 1 }}>Additional Info: <strong>{cellInfo.cpjson}</strong></p>
                                    </div>
                                  }
                                  {
                                    <div key={tq.tqid} style={{ paddingBottom: 12 }}>
                                      <div>
                                        {
                                          // tq.ic || isOptionalQuestion
                                          // ?
                                          // <p style={{ lineHeight: 1, color: !isOptionalQuestion ? 'green' : undefined }}>Answers: {tq.a.length > 0 ? tq.a.join(', ') : '-'}</p>
                                          <p>Answers: {tq.a.length > 0 ? tq.a.join(', ') : '-'}</p>
                                          // :
                                          // <>
                                          //   <p style={{ lineHeight: 1, color: 'red' }}>Answers: {tq.a.length > 0 ? tq.a.join(', ') : '-'}</p>
                                          //   <p style={{ lineHeight: 1 }}>Correct Answers: {tq.ca.length > 0 ? tq.ca.join(', ') : '-'}</p>
                                          // </>
                                        }
                                      </div>
                                    </div>
                                  }
                                </div>
                              )
                            })
                          }
                        </div>
                      </div>
                    </div>
                  )
                })
              }
            </div>
          </ModalBody>
          <ModalFooter>
            <Button color='primary' onClick={() => this.setState({isModalOpened: false})}>Back</Button>
          </ModalFooter>
        </Modal>
      </div>
    );
  }
}

export default AdminReportPage;