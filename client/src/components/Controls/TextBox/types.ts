import { IControlSelectionValue } from '../types'

export interface ITextBoxProps {
  id: number,
  testQuestionId: number,
  questionId: number,
  label: string,
  defaultValue?: string,
  answerId?: number,
  parentQuestionId?: number,
  cellId?: number,
  onChange?: (radio: IControlSelectionValue) => void
}