import { IControlSelectionValue, IControlChildSelectionValue } from '../types'
import { QuestionType } from '~/common/interfaces/apis/question';

export interface IRadioButtonProps {
  answerId: number,
  name?: string,
  value: string,
  label: string,
  hasOpinionAnswer?: boolean,
  parentId: string | number,
  isSelected?: boolean,
  required: boolean,
  onRadioClicked?: (radio: IControlChildSelectionValue) => void
}

export interface IRadioButtonsProps {
  className?: string,
  radios: IRadioButtonProps[],
  hasOpinionSelection: boolean,
  testQuestionId: number,
  questionId: number,
  questionType: QuestionType,
  title: string,
  cellId: number,
  defaultValue?: IRadioCorrectAnswer,
  onSelected: (value: IControlSelectionValue) => void
}

export interface IRadioCorrectAnswer {
  answerId: number,
  answerTitle: string
}