import React from 'react';
import Radio from './radio';
import { IRadioButtonsProps, IRadioCorrectAnswer }  from './types';
import { IControlSelectionValue, IControlChildSelectionValue } from '../types';
import { generateUniqueId } from 'utils/generator';
import { QuestionType } from 'interfaces/apis/question';

// interface IRadioButtonsState {
//   selectedRadioAnswer: IRadioCorrectAnswer
// }

export class RadioButtons extends React.PureComponent<IRadioButtonsProps> {

  onRadioSelected = (radio: IControlChildSelectionValue) => {

    const { onSelected, cellId, testQuestionId, questionId } = this.props;

    onSelected({
      ...radio,
      parentQuestionId: undefined, // No parent at the moment
      cellId,
      testQuestionId,
      questionId
    });
  }

  render() {

    const { className, title, radios, hasOpinionSelection, defaultValue, questionType } = this.props;
    // const { selectedRadioAnswer } = this.state;
    const uniqueId = generateUniqueId();

    return (
      <div className={`form-group ${className ? className : ''}`}>
        <strong><label>{title}</label></strong>
        {
          radios.length > 0
          &&
          radios.map((radio, index) => {

            // Temporary hard code last answer with open ended answer
            let hasOpinionAnswer = hasOpinionSelection && index + 1 === radios.length;

            return (
              <Radio
                parentId={uniqueId}
                name={title}
                key={radio.answerId}
                answerId={radio.answerId}
                label={radio.label}
                value={radio.value}
                onRadioClicked={this.onRadioSelected}
                required={questionType !== QuestionType.RadioButtonsOptional}
                isSelected={!!defaultValue && radio.answerId === defaultValue.answerId}
                hasOpinionAnswer={hasOpinionAnswer}
              />
            )
          })
        }
      </div>
    );
  }
}

export default RadioButtons;