import React, { ChangeEvent } from 'react';
import { IRadioButtonProps } from './types';

interface IRadioButtonState {
  opinionText: string
}

export class RadioButton extends React.PureComponent<IRadioButtonProps, IRadioButtonState> {

  state: IRadioButtonState = {
    opinionText: ''
  }

  componentDidMount() {
    this.setState({
      opinionText: this.props.isSelected ? this.props.value : ''
    });
  }

  onClick = (e: ChangeEvent<HTMLInputElement>) => {
    this.props.onRadioClicked({
      ...this.props,
      ...(this.props.hasOpinionAnswer
        ? { value: this.state.opinionText || e.target.value } 
        : { value: e.target.value }),
    });
  }

  onOpinionTextFieldChanged = (e: ChangeEvent<HTMLInputElement>) => {
    if (this.props.isSelected) {
      this.setState({
        opinionText: e.target.value
      });
      this.props.onRadioClicked({
        ...this.props,
        value: e.target.value || this.props.value
      })
    }
  }

  

  render() {

    const { hasOpinionAnswer, label, name, value, isSelected, answerId, parentId, required } = this.props;

    return (
      <div className="form-check">
        <input className="form-check-input" onChange={this.onClick} type="radio" id={`${parentId}_${answerId}`} name={`${parentId}`} defaultChecked={isSelected} defaultValue={value} required={required}/>
        <label className="form-check-label" htmlFor={`${parentId}_${answerId}`}>
          {label}
        </label>
        {hasOpinionAnswer && <input className="form-control" type='text' style={{ 
          padding: '0.375rem 0.5rem', 
          display: 'inline',
          height: 25, 
          maxWidth: 350, 
          marginLeft: '10px' }} disabled={!isSelected} onChange={this.onOpinionTextFieldChanged} defaultValue={''} /> }
      </div>
    );
  }
};

export default RadioButton;