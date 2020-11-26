import * as React from 'react';

import { IFormControlProps } from '../Controls/types';
import { IQuestion, QuestionType } from 'interfaces/apis/question';

import RadioButtons from '../Controls/RadioButtons';
import { IRadioButtonProps, IRadioCorrectAnswer } from '../Controls/RadioButtons/types';

import CheckBoxes from '../Controls/CheckBoxes';
import { ICheckBoxProps } from '../Controls/CheckBoxes/types';

import TextField from '../Controls/TextBox';

import { IControlSelectionValue } from 'components/Controls/types';

interface ICreateQuestionProps {
  hideQuestionTitle?: boolean,
  onAnswering: (selected: IControlSelectionValue) => void
}

const CreateQuestion: React.FunctionComponent<IQuestion & ICreateQuestionProps> = (props) => {

    const onAnswering = (selection: IControlSelectionValue) => {
      props.onAnswering(selection);
    }

    const { hideQuestionTitle } = props;

    switch (props.qtid) {

      case QuestionType.RadioButtonsWithOpenEndedQuestion:
      case QuestionType.RadioButtonsOptional:
      case QuestionType.RadioButtons:

        var radios: IRadioButtonProps[] = props.a.map((ans) => {
          return {
            required: false,
            answerId: ans.aid,
            parentId: undefined,
            label: ans.at,
            value: ans.at,
            nextQuestionId: ans.nqid
          } as IRadioButtonProps;
        });

        let correctAnswer = props.a.filter(ans => ans.ica)[0];
        let radioCorrectAnswer: IRadioCorrectAnswer = correctAnswer && {
          answerId: correctAnswer.aid,
          answerTitle: correctAnswer.at
        }

        return (
          <RadioButtons
            title={hideQuestionTitle ? '' : props.qt}
            onSelected={onAnswering}
            testQuestionId={props.tqid}
            questionType={props.qtid}
            questionId={props.qid}
            radios={radios}
            defaultValue={radioCorrectAnswer}
            hasOpinionSelection={props.qtid === QuestionType.RadioButtonsWithOpenEndedQuestion}
            cellId={props.cid}
          />
        )
      break;

      case QuestionType.CheckBoxes:

        var checkBoxes: ICheckBoxProps[] = props.a.map((ans) => {
          return {
            answerId: [ans.aid],
            label: ans.at,
            value: ans.at,
            isSelected: ans.ica
          } as ICheckBoxProps;
        })

        return (
          <CheckBoxes
            title={hideQuestionTitle ? '' : props.qt}
            hidden={!!props.qpid}
            testQuestionId={props.tqid}
            parentQuestionId={props.qpid}
            questionId={props.qid}
            onSelected={onAnswering}
            checkBoxes={checkBoxes}
            cellId={props.cid}
          />
        )
      break;

      case QuestionType.NormalTextField:
        return (
        <TextField
          onChange={onAnswering}
          label={props.qt}
          testQuestionId={props.tqid}
          questionId={props.qid}
          id={props.tqid}
        />
        )
      break;
    }
  
} 

export { CreateQuestion }

