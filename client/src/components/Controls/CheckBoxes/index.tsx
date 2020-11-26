import React from 'react';
import Checkbox from './checkbox';
import { ICheckBoxesProps } from './types';
import { IControlSelectionValue, IControlChildSelectionValue } from '../types';

interface ICheckBoxesState {
  selectedAnswers: number[]
}

export default class CheckBoxes extends React.PureComponent<ICheckBoxesProps, ICheckBoxesState> {

  // const style = {
  //   display: props.hidden ? 'block' : 'block'
  // }

  state: ICheckBoxesState = {
    selectedAnswers: []
  }

   onCheckBoxChecked = (checkbox: IControlChildSelectionValue) => {

    const {selectedAnswers} = this.state;

    let newSelectedAnswers = selectedAnswers;

      if (selectedAnswers.findIndex(x => x === checkbox.answerId) < 0)
      newSelectedAnswers.push(checkbox.answerId)
      else
      newSelectedAnswers = selectedAnswers.filter(x => x !== checkbox.answerId)

    this.setState({
      selectedAnswers: newSelectedAnswers
    }, () => {
      this.props.onSelected({
        ...checkbox,
        answerId: checkbox.answerId,
        testQuestionId: this.props.testQuestionId,
        parentQuestionId: this.props.parentQuestionId,
        cellId: this.props.cellId,
        questionId: this.props.questionId
      });
    })
  }

  render(){

    const {checkBoxes, title, className, testQuestionId} = this.props;

    return (
      <div id={`q_${testQuestionId}`} className={`form-group ${className ? className : ''}`}>
        <strong><label>{title}</label></strong>
        {
          checkBoxes.length > 0
          &&
          checkBoxes.map((checkbox) => {
            return (
              <Checkbox
                name={title}
                key={checkbox.answerId}
                answerId={checkbox.answerId}
                label={checkbox.label}
                value={checkbox.value}
                isSelected={checkbox.isSelected}
                onCheckBoxChecked={this.onCheckBoxChecked}
              />
            )
          })
        }
      </div>
    );
  }
  
}