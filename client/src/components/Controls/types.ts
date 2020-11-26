export interface IControlSelectionValue {
  answerId: number | number[],
  testQuestionId: number,
  questionId: number,
  value: string,
  parentQuestionId: number,
  cellId: number
}

export interface IControlChildSelectionValue {
  answerId: number,
  value: string,
}

export interface IFormControlProps {
  onSelected: (value: IControlSelectionValue | IControlSelectionValue[] ) => void
}

export interface IFormField<IControlType> {
  title: string,
  control: IControlType
}