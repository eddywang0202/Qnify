import React, { PureComponent } from 'react';
import { IDemographicQuestionsResponse } from 'interfaces/apis/question'
import { CreateQuestion } from 'components/Form';
import { IControlSelectionValue } from 'components/Controls/types';
import { GET, POST } from 'server';

import queryString from 'query-string';
import { IDemographicAnswerInsertRequest, Path } from 'interfaces/apis/question';
import Auth from 'components/Auth';
import { Path as TestPath, IGetTestInfoResponse, IGetConsentResponse } from 'interfaces/apis/test';

import BraftEditor from 'braft-editor';
import 'braft-editor/dist/index.css';

interface IRegisterState {
  formQuestions: IDemographicQuestionsResponse,
  formData: {
    tqid: number,
    value: string
  }[],
  isFetching: boolean,
  isSubmitting: boolean,
  editorState: any,
  agreeConsent: boolean,
  // activeTestInfo?: IGetTestInfoResponse
}

class Register extends React.Component<{}, IRegisterState> {

  state: IRegisterState = {
    formQuestions: [],
    formData: [],
    isFetching: false,
    isSubmitting: false,
    editorState: BraftEditor.createEditorState(null),
    agreeConsent: false,
    // activeTestInfo: undefined
  }

  // If you get the error about can't perform blablabal refer to this thread 
  // https://github.com/airbnb/javascript/issues/684
  async componentDidMount() {
    this.setState({
      isFetching: true
    });

    try {
      const resp = await GET<IDemographicQuestionsResponse>(Path.GetDemographicQuestionsRequestPath);
      this.setState({
        formQuestions: resp.data,
        formData: resp.data.map((q) => {
          return {
            tqid: q.tqid,
            value: ''
          }
        }),
        isFetching: false,
        isSubmitting: false
      });

      // Get active test
      const activeTest = (await GET<IGetTestInfoResponse>(TestPath.GetActiveTestInfo)).data;

      // this.setState({
      //   activeTestInfo: activeTest
      // })

      const consent = (await GET<IGetConsentResponse>(TestPath.GetTestConsent(activeTest.tid))).data;
      this.setState({
        editorState: BraftEditor.createEditorState(consent)
      });
    }
    catch (err) {
      console.error(err);
    }
  }

  onControlValueSelect = (selected: IControlSelectionValue) => {

    this.setState((prevState) => {

      var newFormData = prevState.formData.map(d => {
        if (d.tqid === selected.testQuestionId)
          return {
            ...d,
            value: selected.value
          }
        return d
      });

      var newFormQuestions = prevState.formQuestions.map(q => {
        if (q.qid === selected.questionId) {
          return {
            ...q,
            a: q.a.map(a => {
              if (a.aid === selected.answerId) a.ica = true;
              else a.ica = false;

              return a;
            })
          }
        }
        else return q;
      })

      return {
        ...prevState,
        formData: newFormData,
        formQuestions: newFormQuestions
      }
    });

    
  }

  toggleAgreeConsent = (toggleChecked: boolean) => {
    this.setState({
      agreeConsent: toggleChecked
    })
  }

  onFormSubmit = async (e) => {

    this.setState({
      isSubmitting: true
    })

    e.preventDefault();

    const { location } = this.props;
    const { formData } = this.state;

    const qs = queryString.parse(location.search);

    try{
      let resp = await POST<IDemographicAnswerInsertRequest, string>(Path.DemographicAnswerRequestPath, {
        // tid: activeTestInfo.tid,
        et: qs.etk,
        uqas: formData.map((qa) => {
          return {
            tqid: qa.tqid,
            a: qa.value
          }
        })
      });
      if (resp.data) {
        Auth.setAccessToken(resp.data);
        this.props.history.push(`/`);
      }
    }catch(err){
      this.setState({
        isSubmitting: false
      })
    }
  }

  render() {

    const { formQuestions, isFetching, isSubmitting, editorState, agreeConsent } = this.state;

    return (
    <div className={'register'} style={{ paddingBottom: 100 }}>
      {isFetching
      ?
        <h4>Loading...</h4>
      :
        <>
          <h3>First thing first, tell us about yourself</h3>
          <form onSubmit={this.onFormSubmit}>
            {
              formQuestions.length > 0
              &&
              formQuestions.map(q => {
                return (
                  <CreateQuestion
                    key={q.qid}
                    onAnswering={this.onControlValueSelect}
                    {...q}
                  />
                )
              })
            }
            <div style={{backgroundColor: '#444'}} className={'card'}>
              <div className={'card-body'}>
                <div className={'bf-container'}>
                  <div className={'public-DraftEditor-content'}>
                      <div dangerouslySetInnerHTML={{
                        __html: editorState.toHTML()
                      }}></div>
                  </div>
                </div>
              </div>
            </div>
            <div style={{margin: '16px 0', textAlign: 'center'}}>
              <label>
                <input type='checkbox' onChange={() => this.toggleAgreeConsent(!agreeConsent)} defaultChecked={agreeConsent}/>
                <span style={{marginLeft: 12}}>Yes, I'd agreed the above rules and terms.</span>
              </label>
              <br/>
              <button disabled={isSubmitting || !agreeConsent} className={'btn btn-success'}>Proceed</button>
            </div>
          </form>
        </>
      }
    </div>
    )
  }
}

export default Register;