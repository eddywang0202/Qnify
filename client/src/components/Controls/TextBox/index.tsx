import React, { ChangeEvent } from 'react';
import { ITextBoxProps } from './types';

const TextBox = (props: ITextBoxProps) => {

  const onChange = (e: ChangeEvent<HTMLInputElement>) => {
    props.onChange({
      ...props,
      value: e.target.value || '',
      answerId: undefined,
      parentQuestionId: undefined,
      cellId: undefined
    })
  }

  let inputType: string = 'text';

  if (props.testQuestionId == 4) inputType = 'email';

  return (
    <div className="form-group">
      <strong><label htmlFor={props.id.toString()}>{props.label}</label></strong>
      <input style={{maxWidth: '350px'}} type={inputType} className="form-control" onChange={onChange} id={props.id.toString()} defaultValue={props.defaultValue} required/>
      <div className="invalid-feedback">
        This field is required.
      </div>
    </div>
  );
};

export default TextBox;