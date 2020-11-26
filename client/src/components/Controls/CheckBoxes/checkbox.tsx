import React from 'react';
import { ICheckBoxProps } from './types';
import { generateUniqueId } from 'utils/generator';

const CheckBox = (props: ICheckBoxProps) => {

  const onClick = (e: React.MouseEvent) => {

    props.onCheckBoxChecked(props);
  }

  const uniqueId = generateUniqueId();

  return (
    <div className="form-check">
      <input className="form-check-input" onClick={onClick} type="checkbox" defaultChecked={props.isSelected} value={props.value} id={uniqueId} />
      <label className="form-check-label" htmlFor={uniqueId}>
        {props.label}
      </label>
    </div>
  );
};

export default CheckBox;