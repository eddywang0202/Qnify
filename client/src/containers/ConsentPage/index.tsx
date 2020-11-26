import React, { PureComponent } from 'react';
import BraftEditor from 'braft-editor';
import 'braft-editor/dist/index.css';
import { GET, POST } from 'server';
import { Path, IGetTestInfoResponse, IUpdateConsentRequest, IUpdateConsentResponse, IGetTestInfo } from 'interfaces/apis/test';

class ConsentPage extends PureComponent {

  state = {
    editorState: BraftEditor.createEditorState(null),
    isFetching: false,
    isSubmitting: false,
    disabled: false,
    selectedTestId: undefined,
    lastSavedDateTime: undefined,
    testList: []
  }

  async componentDidMount() {
    // Get existing html content
    let resp = await GET<IGetTestInfoResponse[]>(Path.GetAllTestInfo);

    this.setState({
      testList: resp.data
    })
  }

  async onTestDropdownClick(testId: number) {
    
    this.setState({
      isFetching: true,
      selectedTestId: testId,
      disabled: true,
    })

    if (!!testId) { 
      try {
        // Get existing html content
        const consentString = (await GET<string>(Path.GetTestConsent(testId))).data;

        this.setState({
          editorState: BraftEditor.createEditorState(consentString),
          disabled: false
        });
      } catch (err) {
        console.error('Error while fetching test info.' + err);
      }
    }

    this.setState({
      isFetching: false
    })
  }

  onEditorChanged = (editorState) => {
    this.setState({ editorState });
  }

  onEditorSave = async () => {

    const htmlContent = this.state.editorState.toHTML();

    this.setState({
      isSubmitting: true
    })

    try {
      let stringifyHtmlContent = JSON.stringify(htmlContent);

      const result = await POST<IUpdateConsentRequest, IUpdateConsentResponse>(Path.UpdateTestConsent(this.state.selectedTestId), stringifyHtmlContent);

      const curDateTimeString = new Date().toDateString() + ', at ' + new Date().toLocaleTimeString()

      if (result.data) {
        this.setState({
          lastSavedDateTime: curDateTimeString
        })
      }

    } catch (err) {
      console.error('Error while submitting consent content update.', err);
    }

    this.setState({
      isSubmitting: false
    })
  }

  render() {

    const { editorState, isSubmitting, isFetching, disabled, lastSavedDateTime, selectedTestId, testList } = this.state;

    return (
      <div>
        <div>
          <label>
            Select a test:
            <select className={'form-control'} value={selectedTestId} onChange={(e) => this.onTestDropdownClick(Number(e.target.value))}>
              <option value={undefined}>--- Select a test ---</option>
              {
                testList.map(test => <option key={test.tid} value={test.tid}>{test.t}</option>)
              }
            </select>
          </label>
        </div>
        {
          !selectedTestId
          ?
          <div>Please select a test to edit.</div>
          :
          <div>
            <BraftEditor
              language={'en'}
              value={editorState}
              onChange={this.onEditorChanged}
              onSave={this.onEditorSave}
              readOnly={isFetching}
            />
            <hr />
            <div className='row'>
              <div className={'col-sm-6'}>
                <button onClick={this.onEditorSave}
                  disabled={isSubmitting || isFetching || disabled || !selectedTestId}
                  className={'btn btn-success'}>
                  {(isSubmitting ? 'Saving...' : 'Save [Ctrl]+[S]')}
                </button>
                <div>{!!lastSavedDateTime && <i>Last saved was on {lastSavedDateTime}.</i>}</div>
              </div>
              <div className={'col-sm-6'}>
                <h5>Hint</h5>
                <div style={{fontSize: 10}}>Insert Line break: <b>[Shift]+[Enter] x 2 (twice)</b></div>
                <div style={{ fontSize: 10 }}>Undo / Redo: <b>[Ctrl]+[Z] / [Ctrl]+[Y]</b></div>
                <div style={{ fontSize: 10 }}><i>Only add image from the internet with image url, not from your PC's</i></div>
              </div>
            </div>
          </div>
        }
      </div>
    );
  }
}


export default ConsentPage;