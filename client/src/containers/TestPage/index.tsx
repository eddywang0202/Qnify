import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { findLastIndex } from 'utils/array';
import { subscribeMagnifierToImage } from 'utils/eventHandler';
import Slider from 'react-rangeslider';
import Carousal from 'components/Slider'
import { IImageModal } from '../ManageImage/types';
import ItemList from 'components/Items';

import * as Actions from './action';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { ITestQuestionPage } from './types';
import { IQuestionGroupType, ICell, IQuestion, IGetTestQuestionListResponse, QuestionType } from 'interfaces/apis/question';
import { CreateQuestion } from 'components/Form';
import { IControlSelectionValue } from 'components/Controls/types';
import Grid from 'components/Grid';
import { IItemProps } from '~/components/Items/types';
import Collapsible from 'react-collapsible';

function mapStateToProps(state) {
  return {
    ...state.testPage
  };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(
    {
      ...Actions
    }, dispatch);
}

class TestPage extends Component<ITestQuestionPage> {

  slider = React.createRef<Slider>();

  onNextClick = async (e: React.FormEvent) => {
    
    e.preventDefault();
    const { testSetList, selectedTestSetId, cells, testSetQuestions } = this.props;
    let currentTestSetIndex = testSetList.findIndex(x => x.tsid === selectedTestSetId);

    let notAnsweredQuestion: IQuestion = undefined;
    testSetQuestions.forEach(q => {
      if (!q.a.some(ans => ans.ica) && q.qtid === QuestionType.RadioButtons){
        notAnsweredQuestion = q;
      }
      if (!!notAnsweredQuestion) return;
    });

    if (!!notAnsweredQuestion) {
      let questionCellId = notAnsweredQuestion.cid;
      let imageID = cells.find(c => c.cid === questionCellId).acpjson;
      alert(`Image ID ${imageID} has not been evaluated yet.`);
      return;
    }

    let lastQuestion: boolean = (currentTestSetIndex === testSetList.length - 1);
    let nextQuestionIndex = (lastQuestion) ? currentTestSetIndex : currentTestSetIndex + 1;
    
    await this.submitAndGoToQuestion(testSetList[nextQuestionIndex].tsid, lastQuestion);

    // If is not the last question
    if (lastQuestion) {
      this.props.history.push('/result');
    }
  }

  onPrevClick = async (e: React.MouseEvent) => {

    e.preventDefault();
    const { testSetList, selectedTestSetId } = this.props;
    let currentTestSetIndex = testSetList.findIndex(x => x.tsid === selectedTestSetId);
    await this.submitAndGoToQuestion(testSetList[currentTestSetIndex - 1].tsid)
  }

  submitAndGoToQuestion = async (nextTestSetId: number, submitOnly: boolean = false) => {

    const { OnFormSubmit, ToggleIsSubmitting, SetSelectedTestSetId, selectedTestSetId } = this.props;

    if(!submitOnly) SetSelectedTestSetId(nextTestSetId);

    ToggleIsSubmitting(true);

    let cont = true;

    try {
      await OnFormSubmit(selectedTestSetId, this.props.formData);
    }
    catch (err) {
      console.error(err);
      // cont = confirm('Answer couldnt submit while we transitioning your question, do you wish to continue without changes for this test?')
    }
    finally {
      ToggleIsSubmitting(false);
    }

    if (cont && !submitOnly) {
      // Fetch next question
      this.selectTestQuestionsById(nextTestSetId);
    }
  }

  toggleImageModel = (toggle: boolean, modal?: IImageModal) => {
    if (toggle) {
      this.props.SetModal(modal);
    }
    else this.props.ResetModal();
  }

  selectTestQuestionsById = async (testSetId: number) => {
    this.props.ResetOnFetchTestQuestion();
    this.props.ToggleIsFetching(true);
    await this.props.GetTestSetQuestionsById(testSetId);
    this.props.ToggleIsFetching(false);
  }

  onListItemClick = async (item: IItemProps) => {
    await this.submitAndGoToQuestion(item.id)
  }

  onBrightnessSliderChanged = (value: number) => {
    this.props.ChangeModalImageBrightness(value);
    subscribeMagnifierToImage('modal-image', this.props.modal.zoomLevel, value, this.props.modal.contrast)
  }

  onContrastSliderChanged = (value: number) => {
    this.props.ChangeModalImageContrast(value);
    subscribeMagnifierToImage('modal-image', this.props.modal.zoomLevel, this.props.modal.brightness, value)
  }

  onZoomSliderChanged = (value: number) => {
    this.props.ChangeModalImageZoomLevel(value);
    subscribeMagnifierToImage('modal-image', value, this.props.modal.brightness, this.props.modal.contrast)
  }

  onResetImageSettingClicked = () => {
    this.props.ResetModalImageAdjustment();
    subscribeMagnifierToImage('modal-image', 2, 1, 1)
  }

  onAnswerSelect = (controlValue: IControlSelectionValue) => {
    this.props.OnAnswerSelected(controlValue);
  }

  populateCell: (cell: ICell, questions: IQuestion[]) => JSX.Element = (cell: ICell, questions: IQuestion[]) => {
    return (
      <div className={'cell row mask'}>
        <div className={'col-sm-12'}>
          <div className={'tags'}>
            {(JSON.parse(cell.cpjson) as { name: string, value: string }[]).map((kvp, i) => <label key={i}>{!!kvp.name ? `${kvp.name}:` : ''} {kvp.value}</label>)}
            <label>Image ID: {cell.acpjson}</label>
          </div>
          <img src={cell.cimg} onClick={() => this.toggleImageModel(true, {
            header: cell.cimg,
            imageUrl: cell.cimg,
            questions,
            info: [cell.cpjson, cell.acpjson]
          })} />
        </div>
      </div>
    )
  }

  async componentDidMount() {
    //Check if answer has already answered question

    this.props.ToggleIsFetching(true);

    await this.props.GetTestSetQuestionList();

    let hasAllAnswered = true;
    this.props.testSetList.forEach(ts => hasAllAnswered = hasAllAnswered && ts.ia);

    if (hasAllAnswered && this.props.testSetList.length > 0) {
      this.props.history.push('/result');
    }
    else {
      if (this.props.selectedTestSetId != 0) {
        await this.props.GetTestSetQuestionsById(this.props.selectedTestSetId);
      }
    }

    this.props.ToggleIsFetching(false);
  }

  getItems = (testSetList: IGetTestQuestionListResponse[]) => {
    let no = 0;
    // for(let i = 0; i < 25; i++){
    //   testSetList.push({
    //     tsid: i+15,
    //     tso: i+5,
    //     tst: new Date().toDateString()
    //   })
    // }

    // We do not put disable on current question and previous questions.

    let { selectedTestSetId, isFetching, isFormSubmitting } = this.props;
    let selectedTestSetIndex = testSetList.findIndex(ts => ts.tsid === selectedTestSetId);
    let lastIsAnsweredTestQuestionIndex = findLastIndex(testSetList, 'ia', true);

    return testSetList.map((set, i) => {
      let item: IItemProps = {
        id: set.tsid,
        title: `${++no}. ${set.tst}`,
        order: set.tso,
        classNames: set.ia && set.tsid !== selectedTestSetId ? 'list-group-item-success' : '',
        disabled: isFormSubmitting || isFetching || (!set.ia && (i !== selectedTestSetIndex && i !== lastIsAnsweredTestQuestionIndex + 1))
      }
      return item;
    }).sort((a, b) => a.order - b.order);
  }

  onLayoutFilter = (e: React.MouseEvent<HTMLDivElement>) => {
    e.preventDefault();
    const datasetRow = Number(e.target.dataset.rows);
    let filterRows: number[] = [datasetRow];
    this.props.HideRowIndices(filterRows);
  }

  render() {

    const { testSetList, testSetQuestions, selectedTestSetId, testSetTitle, cells, modal, isModalOpened,
      isFetching, isFormSubmitting, hideRowsIndices } = this.props;
    const { brightness, contrast, zoomLevel } = modal;

    let cellQuestions = testSetQuestions.filter(q => q.qgid === IQuestionGroupType.TestCaseQuestionGroup);
    let testMainQuestion = testSetQuestions.filter(q => q.qgid === IQuestionGroupType.TestSetQuestionGroup);

    let className = '';
    if (isFormSubmitting) {
      className += 'submitting'
    }

    return (
      <div className='testPage'>
        <div className={'layout-two-sided'}>
          <div className={'first'} style={{
            flexBasis: '250px',
            height: 'calc(100% - 150px)',
            position: 'fixed',
          }}>
            <h2>Test Cases</h2>
            <div style={{
              height: '100%',
              overflowY: 'auto'
            }}>
              <ItemList
                classNames={''}
                items={this.getItems(testSetList)}
                selectedItemId={selectedTestSetId}
                onItemClick={this.onListItemClick}
              />
            </div>
          </div>
          <div className={'second'} style={{
            flexBasis: 'calc(100% - 250px)',
            paddingLeft: 250
          }}>
            {
              testSetQuestions.length > 0 && !isFetching
                ?
                <div>
                  {isFormSubmitting && <h2>{(selectedTestSetId === testSetList[testSetList.length - 1].tsid) ? 'Generating your result' : 'Saving...'}</h2>}
                  <form id={`form_tsid_${selectedTestSetId}`} className={className} onSubmit={this.onNextClick}>
                    <div style={{ display: 'flex', justifyContent: 'space-between'}}>
                      <h2>{testSetTitle} </h2>
                      <div>
                        <strong style={{ marginRight: 6 }}>Filter View</strong>
                        <div className="btn-group btn-group-toggle" data-toggle="buttons" onClick={this.onLayoutFilter}>
                          <label data-rows={1} className={`btn btn-secondary ${(hideRowsIndices[0] === 1 ? 'active' : undefined)}`}>
                            <input type="radio" name="options" id="option1" autoComplete="off" /> RCC+LCC
                          </label>
                          <label data-rows={0} className={`btn btn-secondary ${(hideRowsIndices[0] === 0 ? 'active' : undefined)}`}>
                            <input type="radio" name="options" id="option1" autoComplete="off"/> RMLO+LMLO
                          </label>
                          <label data-rows={-1} className={`btn btn-secondary ${(hideRowsIndices[0] === -1 ? 'active' : undefined)}`}>
                            <input type="radio" name="options" id="option1" autoComplete="off" /> All
                          </label>
                        </div>
                      </div>
                    </div>
                    <small style={{color: 'yellow'}}><i>* Click each image to evaluate</i></small>
                    <Grid
                      cellClassName={'col--block'}
                      rows={2}
                      columns={2}
                      hideRowIndices={hideRowsIndices}
                      elements={
                        cells.map((c) => {
                          return this.populateCell(c, cellQuestions.filter(q => q.cid === c.cid))
                        })
                      }
                    />
                    {
                      testMainQuestion.map(q => {
                        return <CreateQuestion
                          {...q}
                          key={q.qid}
                          onAnswering={this.onAnswerSelect}
                        />
                      }
                      )
                    }
                    <div className={'flex space-between'}>
                      {<button
                        className='btn btn-secondary'
                        style={{
                          visibility: (testSetQuestions && testSetList[0].tst !== testSetTitle) ? 'visible' : 'hidden'
                        }}
                        disabled={isFormSubmitting}
                        onClick={this.onPrevClick}
                      >
                        {'< Back to Previous Test Case'}
                      </button>}
                      <button
                        form={`form_tsid_${selectedTestSetId}`}
                        disabled={isFormSubmitting}
                        className='btn btn-success'
                      // onClick={this.onNextClick}
                      >
                        {((selectedTestSetId === testSetList[testSetList.length - 1].tsid) ? 'Submit Your Answers' : 'Next Test Case >')}
                      </button>
                    </div>
                  </form>
                </div>
                :
                <h2>{'Loading...'}</h2>
            }
          </div>
        </div>
        <Modal isOpen={isModalOpened} onOpened={() => subscribeMagnifierToImage('modal-image', this.props.modal.zoomLevel, this.props.modal.brightness, this.props.modal.contrast)} toggle={() => this.toggleImageModel(false)} className={'modal-image'}>
          <ModalHeader toggle={() => this.toggleImageModel(false)}>{modal.imageUrl}</ModalHeader>
          <ModalBody>
            <div className={'row'}>
              <div className={'col-sm-8 mask'}>
                <div className="img-magnifier-container">
                  <div className={'tags'}>
                    {modal.info.length > 0 && modal.info.map((jsonString, i) => {
                      if (i == modal.info.length - 1) return 'Image ID: ' + jsonString;
                      else return (JSON.parse(jsonString) as { name: string, value: string }[]).map((kvp, i) => <label key={i}>{!!kvp.name ? `${kvp.name}:` : ''} {kvp.value}</label>)
                    })}
                  </div>
                  <img id='modal-image' src={modal.imageUrl} style={{
                    filter: `brightness(${brightness}) contrast(${contrast})`
                  }} />
                </div>
              </div>
              <div className={'col-sm-4'}>
                <Collapsible trigger="Image Evaluation">
                  {
                    modal.questions.map(q => {
                      return (
                        <CreateQuestion
                          hideQuestionTitle={q.qid === 17} //temporary hiding PGMI question
                          key={q.qid}
                          {...q}
                          onAnswering={this.onAnswerSelect}
                        />
                      )
                    })
                  }
                </Collapsible>
              </div>
            </div>
            <div style={{marginTop: 25}}>
              <h2>Image Settings</h2>
              <hr />
              <strong>Brightness <span style={{ float: 'right' }}>{(brightness * 100).toFixed(0)}%</span></strong>
              <Slider
                min={0}
                max={2}
                step={0.01}
                tooltip={false}
                value={brightness}
                onChange={this.onBrightnessSliderChanged}
              />
              <strong>Contrast <span style={{ float: 'right' }}>{(contrast * 100).toFixed(0)}%</span></strong>
              <Slider
                min={0}
                max={2}
                step={0.01}
                tooltip={false}
                value={contrast}
                onChange={this.onContrastSliderChanged}
              />
              <strong>Zoom Level <span style={{ float: 'right' }}>x {(zoomLevel).toFixed(2)}</span></strong>
              <Slider
                min={1}
                max={10}
                step={1}
                tooltip={false}
                value={zoomLevel}
                onChange={this.onZoomSliderChanged}
              />
              <button className={'btn btn-primary'} onClick={this.onResetImageSettingClicked}>Reset</button>
            </div>
          </ModalBody>
          <ModalFooter>
            <Button color='primary' onClick={() => this.toggleImageModel(false)}>Back to question</Button>
          </ModalFooter>
        </Modal>
      </div>
    );
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(TestPage);