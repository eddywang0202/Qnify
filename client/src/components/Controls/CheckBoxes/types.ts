import { IFormControlProps, IControlSelectionValue, IControlChildSelectionValue } from '../types'

export interface ICheckBoxProps {
  answerId: number[],
  name: string,
  value: string,
  label: string,
  isSelected: boolean,
  onCheckBoxChecked?: (radio: IControlChildSelectionValue) => void
}

export interface ICheckBoxesProps {
  title: string,
  className?: string,
  checkBoxes: ICheckBoxProps[],
  parentQuestionId?: number,
  testQuestionId: number,
  questionId: number,
  hidden: boolean,
  cellId: number,
  onSelected: (value: IControlSelectionValue | IControlSelectionValue[]) => void
}