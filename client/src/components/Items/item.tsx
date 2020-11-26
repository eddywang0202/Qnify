import React from 'react';

import * as Types from './types';

const Item = (props: Types.IItemProps) => {

  const onClick = () => {
    props.onClick && props.onClick(props);
  }

  let classNames = 'list-group-item';
  if (props.onClick) classNames += ' list-group-item-action';
  if (props.classNames) classNames += ` ${props.classNames}`;
  if (props.isSelected) classNames += ' active';
  if (props.disabled) classNames += ' disabled';



  return (
    <a className={classNames} title={props.title} onClick={!props.isSelected ? onClick : undefined}>
      {props.title}
      <div>
        {!!props.onDeleteClick && <button onClick={(e: React.MouseEvent) => props.onDeleteClick(e, props)} style={{
          borderRadius: 16,
          fontSize: 12
        }} className={'btn btn-danger btn-sm'}>Remove</button> }
      </div>
    </a>
  );
};

export default Item;