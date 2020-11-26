import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators, Dispatch } from 'redux';

import ListItems from 'components/Items';
import { IItemProps } from 'components/Items/types';

import Grid from 'components/Grid';
import { CreateQuestion } from 'components/Form';
import { IManageImageState, IImageModal } from './types';
import * as Actions from './action';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

import { subscribeMagnifierToImage } from 'utils/eventHandler';
import Slider from 'react-rangeslider';
import 'react-rangeslider/lib/index.css';
import { IGetTestQuestionListResponse, IQuestionGroupType, IQuestion, ICell, QuestionType } from 'interfaces/apis/question';
import { IControlSelectionValue } from '~/components/Controls/types';

function mapStateToProps(state) {
  return {
    ...state.manageImage
  };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(
    {
      ...Actions
    }, dispatch);
}

export type IManageImage = typeof Actions & IManageImageState;

class ManageImage extends PureComponent<IManageImage> {

  async componentDidMount() {
    await this.props.GetTestList();
  }

  async onTestDropdownClick(testId: number) {
    if (testId) {
      await this.props.GetTestSetList(testId);
    }
  }

  onListItemClick = async (item: IItemProps) => {
    this.props.ResetOnFetchTestQuestion();
    this.props.ToggleIsFetching(true);
    await this.props.GetTestQuestion(item.id);
    this.props.ToggleIsFetching(false);
  }

  onListItemDeleteClick = async (e: React.MouseEvent, item: IItemProps) => {
    e.preventDefault();
    e.stopPropagation();
    let yesno = confirm('Are you sure you want to delete this question?');
    if (yesno) {
      let success = await this.props.DeleteTestQuestion(item.id);
      if (success && this.props.selectedTestSetId === item.id) {
        this.props.ResetOnFetchTestQuestion();
      }
    }
  }

  onCreateNewClick = async () => {

    this.props.ResetOnFetchTestQuestion();
    this.props.ToggleIsFetching(true);
    await this.props.OnCreateNewTestQuestion();
    this.props.ToggleIsFetching(false);
  }

  onAnswerSelect = (controlValue: IControlSelectionValue) => {
    this.props.OnAnswerSelected(controlValue);
  }

  onImageIDChanged = (e: React.FocusEvent<HTMLInputElement>, cell: ICell) => {
    let inputImageId = e.target.value;

    cell.acpjson = inputImageId;

    this.props.OnImageIdChanged(cell);
  }

  onImageUrlChanged = (e: React.FocusEvent<HTMLInputElement>, cell: ICell) => {
    let inputImageUrl = e.target.value;

    cell.cimg = inputImageUrl;

    this.props.OnImageUrlChanged(cell);
  }

  getItems = (testSetList: IGetTestQuestionListResponse[]) => {

    const { isCreateOrUpdating, isFetching } = this.props;

    let no = 0;
    return testSetList.map(set => {
      let item: IItemProps = {
        id: set.tsid,
        title: `${++no}. ${set.tst}`,
        order: set.tso,
        disabled: isFetching || isCreateOrUpdating
      }
      return item;
    }).sort((a, b) => a.order - b.order);
  }

  populateCell: (cell: ICell, question: IQuestion[]) => JSX.Element = (cell: ICell, question: IQuestion[]) => {
    return (
      <div className={'cell row'}>
        <div className={'col-sm-6'}>
          <img src={`${cell.cimg}`} onClick={() => this.toggleImageModel(true, {
            header: cell.cimg,
            imageUrl: cell.cimg,
          })}/>
        </div>
        <div className={'col-sm-6'} style={{padding: 10}}>
          <div className={'form-group'}>
            <label><strong>Image Url</strong></label>
            <input placeholder={'Insert your image link here'} className={'form-control'} onBlur={(e) => this.onImageUrlChanged(e, cell)} defaultValue={cell.cimg} type='text' />
            <small className="form-text text-muted">Some image link may not work, make sure the image link is direct and publicly accessible.</small>
          </div>
          <div className={'form-group'}>
            <label><strong>Image ID</strong></label>
            <input className={'form-control'} onBlur={(e) => this.onImageIDChanged(e, cell)} defaultValue={cell.acpjson} type='text' />
          </div>
        {/* {
          question.map(q => {
            return (
              <CreateQuestion
                key={q.qid}
                {...q}
                onAnswering={this.onAnswerSelect}
              />
            )
          })
        } */}
        </div>
      </div>
    )
  }

  onFormSubmit = async (e) => {
    e.preventDefault();
    const { UpdateForm, SubmitNewForm, onCreateNew, ToggleIsCreateOrUpdating, selectedTestId, selectedTestSetId, formData } = this.props;

    ToggleIsCreateOrUpdating(true);
    if (onCreateNew) {
      await SubmitNewForm(formData);
      await this.props.GetTestSetList(selectedTestId);
      this.props.ResetOnFetchTestQuestion();
    }
    else {
      await UpdateForm({
        ...formData,
        tsid: selectedTestSetId
      });
    }
    ToggleIsCreateOrUpdating(false);
  }

  toggleImageModel = (toggle: boolean, modal?: IImageModal) => {
    if (toggle) {
      this.props.SetModal(modal);
    }
    else this.props.ResetModal();
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

  onLayoutFilter = (e: React.MouseEvent<HTMLDivElement>) => {
    e.preventDefault();
    const datasetRow = Number(e.target.dataset.rows);
    let filterRows: number[] = [datasetRow];
    this.props.HideRowIndices(filterRows); 
  }

  render() {

    const { modal, isModalOpened, testSetList, testSetQuestions, cells, 
      testList, selectedTestSetId, isFetching, selectedTestId, testSetTitle, 
      onCreateNew, isCreateOrUpdating, hideRowsIndices } = this.props;
    const { brightness, contrast, zoomLevel } = modal;
    let cellQuestions = testSetQuestions.filter(q => q.qgid === IQuestionGroupType.TestCaseQuestionGroup);
    let testMainQuestion = testSetQuestions.filter(q => q.qgid === IQuestionGroupType.TestSetQuestionGroup);

    return (
      <div>
        <div style={{float: 'right'}}>
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
        <h1>Manage Test Cases</h1>
        <hr/>
        <div className={'layout-two-sided'}>
          <div className={'first'} style={{
            height: 'calc(100% - 100px)',
            overflowY: 'auto',
            position: 'fixed'
          }}>
            <ListItems
              items={this.getItems(testSetList)}
              onItemClick={this.onListItemClick}
              selectedItemId={selectedTestSetId}
              onDeleteClick={this.onListItemDeleteClick}
              onCreateNewClick={!!selectedTestId && this.onCreateNewClick}
            />
          </div>
          {isFetching && <h2 className={'second'}>{'Loading...'}</h2> }
          {
            testSetQuestions.length > 0
            &&
            <div className={'second'}>
              <form onSubmit={this.onFormSubmit}>
                <div style={{display: 'flex', justifyContent: 'space-between'}}>
                  <label>Test Title
                    <input className={'form-control'} onChange={(e) => this.props.OnTestTitleChanged(e.target.value)} defaultValue={testSetTitle} type='text' required/>
                  </label>
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
                <Grid
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
                  testMainQuestion.filter(q => q.qtid !== QuestionType.RadioButtonsOptional).map(q => {
                    return <CreateQuestion
                        {...q}
                        key={q.qid}
                        onAnswering={this.onAnswerSelect}
                      />
                    }
                  )
                }
                <button disabled={isCreateOrUpdating} className={'btn btn-success'}>{onCreateNew
                  ? ((isCreateOrUpdating) ? 'Creating...' : 'Create')
                  : ((isCreateOrUpdating) ? 'Saving...' : 'Save Changes')  }</button>
              </form>
            </div>
          }
        </div>
        <Modal isOpen={isModalOpened} onOpened={() => subscribeMagnifierToImage('modal-image', this.props.modal.zoomLevel, this.props.modal.brightness, this.props.modal.contrast)} toggle={() => this.toggleImageModel(false)} className={'modal-image'}>
          <ModalHeader toggle={() => this.toggleImageModel(false)}>{modal.imageUrl}</ModalHeader>
          <ModalBody>
            <div className={'row'}>
              <div className={'col-sm-8'}>
                <div className="img-magnifier-container">
                  <img id='modal-image' src={modal.imageUrl} style={{
                    filter: `brightness(${brightness}) contrast(${contrast})`
                  }}/>
                </div>
              </div>
              <div className={'col-sm-4'}>
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
                <strong>Zoom Level <span style={{float: 'right'}}>x {(zoomLevel).toFixed(2)}</span></strong>
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
            </div>
          </ModalBody>
          <ModalFooter>
            <Button color='primary' onClick={() => this.toggleImageModel(false)}>Back</Button>
          </ModalFooter>
        </Modal>
      </div>
    );
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ManageImage);

// imageUrl: '',
