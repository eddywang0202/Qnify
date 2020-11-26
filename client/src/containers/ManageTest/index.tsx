import React, { PureComponent, FormEvent } from 'react';
import ReactTable, { Column, Filter } from 'react-table';
import { Path, IGetTestInfoResponse, IUpdateTestStatusRequest, IUpdateTestStatusResponse, IUpdateTestTitleRequest, IUpdateTestTitleResponse, IInsertTestRequest, IInsertTestResponse, IDeleteTestResponse } from 'interfaces/apis/test';
import { GET, POST, DELETE } from 'server';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { number, string } from 'prop-types';

interface IManageTestState {
  testInfos: IGetTestInfoResponse[],
  isEditModalOpened: boolean,
  isCreateModalOpened: boolean,
  isCreating: boolean,
  editModal: { 
    testId: number,
    testTitle: string,
    status: boolean,
  }
}

class ManageTest extends PureComponent<any, IManageTestState> {

  state: IManageTestState = {
    testInfos: [],
    isEditModalOpened: false,
    isCreateModalOpened: false,
    isCreating: false,
    editModal: {
      testId: undefined,
      testTitle: "",
      status: false
    }
  }

  async componentDidMount() {
    let resp = await GET<IGetTestInfoResponse[]>(Path.GetAllTestInfo);

    this.setState({
      testInfos: resp.data
    });
  }

  onEditModeButtonClicked(testId: number) {

    const { testInfos } = this.state;

    let selectedTest = testInfos.find(t => t.tid === testId)

    this.setState({
      isEditModalOpened: true,
      editModal: {
        testId: selectedTest.tid,
        testTitle: selectedTest.t,
        status: selectedTest.s
      }
    })
  }

  async onRemoveButtonClicked(testId: number) {
    let cfm = confirm(`Are you sure you want to remove test id: ${testId}?`);

    if (cfm) {
      let resp = await DELETE<IDeleteTestResponse>(Path.DeleteTest(testId));

      if (resp.data) {
        this.setState(prevState => {
          return {
            ...prevState,
            testInfos: prevState.testInfos.filter(t => t.tid != testId)
          }
        })
      }
    }
  }

  column: Column[] = [{
      Header: 'Test Id',
      filterMethod: (filter: Filter, rows: any[], column: any) => rows.tid && rows.tid.toLowerCase().indexOf(filter.value.toLowerCase()) >= 0,
      accessor: 'tid',
      Cell: props => props.value || '-'
    },
    {
      Header: 'Test Title',
      filterable: true,
      filterMethod: (filter: Filter, rows: any[], column: any) => rows.t && rows.t.toLowerCase().indexOf(filter.value.toLowerCase()) >= 0,
      accessor: 't',
      Cell: props => props.value || '-'
    },
    {
      Header: 'Status',
      accessor: 's',
      Cell: props => props.value == 1 ? 'Active' : 'Inactive'
    },
    {
      id: 'action',
      Header: 'Action',
      accessor: a => a,
      sortable: false,
      Cell: props => {
        return (
          <div style={{ display: 'inline-block' }} className={'btn-group'} role={'group'}>
            <button onClick={() => this.onEditModeButtonClicked(props.value.tid)} className={'btn btn-primary'}>Edit</button>
            <button onClick={() => this.onRemoveButtonClicked(props.value.tid)} className={'btn btn-danger'}>Delete</button>
          </div>
        )
      }
    }];

  onTestTitleChanged = async (e: React.FocusEvent<HTMLInputElement>) => {

    const { editModal: modal } = this.state;

    let newTitle = e.target.value;

    if (newTitle === modal.testTitle) return;

    let resp = await POST<IUpdateTestTitleRequest, IUpdateTestTitleResponse>(Path.UpdateTestTitle(modal.testId), JSON.stringify(newTitle));

    if (!!resp.data) {
      this.setState((prevState) => {
        return {
          testInfos: prevState.testInfos.map(t => t.tid === modal.testId ? { ...t, t: newTitle } : t),
          editModal: {
            ...prevState.editModal,
            testTitle: newTitle
          }
        }
      });
    }
  }

  onTestStatusChanged = async (e: React.ChangeEvent<HTMLInputElement>) => {

    const { editModal: modal } = this.state;

    let toCheck = e.target.checked;
    let confirmMessage = (e.target.checked)
      ? 'Are you sure you want to set it as current active test? This will affect the current on-going participant test.'
      : 'Are you sure you want to set it to inactive? Participant will not be able to sign in to take test if no test is in active.'

    let ok = confirm(confirmMessage);
    if (ok) {

      let resp = await POST<IUpdateTestStatusRequest, IUpdateTestStatusResponse>(Path.UpdateTestStatus(modal.testId), toCheck);

      if (!!resp.data) {
        this.setState((prevState) => {
          return {
            testInfos: prevState.testInfos.map(t => t.tid === resp.data.tid ? resp.data : { ...t, s: false }),
            editModal: {
              ...prevState.editModal,
              status: !prevState.editModal.status
            }
          }
        })
      }
    }
  }

  onSubmitNewTest = async (e: FormEvent) => {

    e.preventDefault();

    await this.setState({
      isCreating: true
    });
    
    let newTestTitle = (document.getElementById('createTestTitleInput') as HTMLInputElement).value;

    if (!newTestTitle) {
      alert('Test title cannot leave blank.');
      return;
    }
    else
    {
      let resp = await POST<IInsertTestRequest, IInsertTestResponse>(Path.InsertTest, JSON.stringify(newTestTitle));
    
      if(!!resp.data) {
        this.setState(prevState => {
          return {
            ...prevState,
            testInfos: prevState.testInfos.concat(resp.data)
          }
        })
      }
    }

    this.setState({
      isCreating: false,
      isCreateModalOpened: false
    });
  }

  toggleEditModal = (toggle: boolean) => {
    this.setState({
      isEditModalOpened: toggle
    })
  }

  toggleCreateModal = (toggle: boolean) => {
    this.setState({
      isCreateModalOpened: toggle
    })
  }
    
  render() {

    const { testInfos, isEditModalOpened, isCreateModalOpened, editModal, isCreating } = this.state;

    return (
      <div>
        <div style={{ display: 'inline-block', width: '100%' }}>
          <h1 style={{ float: 'left' }}>Manage Test</h1>
          <button style={{ float: 'right' }}
            className={'btn btn-success'} 
            onClick={() => this.toggleCreateModal(true)}
          >
            {'Create New Test'}
          </button>
        </div>
        <ReactTable
          data={testInfos || []}
          columns={this.column}
          defaultPageSize={10}
          showPageSizeOptions={false}
        />
        <Modal isOpen={isEditModalOpened}>
          <ModalHeader toggle={() => this.toggleEditModal(false)}>Edit test - {editModal.testTitle}</ModalHeader>
          <ModalBody>
            <div>
              <label><strong>Test Title</strong></label>
              <input type='text' className={'form-control'} onBlur={this.onTestTitleChanged} defaultValue={editModal.testTitle} />
            </div>
            <hr/>
            <div className="form-check">
              <input type='checkbox' className={'form-check-input'} checked={editModal.status} onChange={this.onTestStatusChanged} />
              <label className="form-check-label">Set to active</label>
            </div>
          </ModalBody>
          <ModalFooter>
            <Button color='primary' onClick={() => this.toggleEditModal(false)}>Done</Button>
          </ModalFooter>
        </Modal>
        <Modal isOpen={isCreateModalOpened} toggle={!isCreating ? (() => this.toggleCreateModal(false)) : undefined}>
          <ModalHeader toggle={() => this.toggleCreateModal(false)}>Create New Test</ModalHeader>
          <ModalBody>
            <form id='createTestForm' onSubmit={this.onSubmitNewTest}>
              <label><strong>Test Title</strong></label>
              <input autoComplete='off' disabled={isCreating} id='createTestTitleInput' type='text' className={'form-control'} required/>
            </form>
          </ModalBody>
          <ModalFooter>
            <Button color='link' disabled={isCreating} onClick={() => this.toggleCreateModal(false)}>Cancel</Button>
            <Button color='success' disabled={isCreating} form='createTestForm'>{isCreating ? 'Creating...' : 'Add New Test'}</Button>
          </ModalFooter>
        </Modal>
      </div>
    );
  }
}

export default ManageTest;