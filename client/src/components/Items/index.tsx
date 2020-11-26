import React, { PureComponent } from 'react';

import Item from './item';
import { IListItemState, IListItemProps, IItemProps } from './types';

class ListItems extends PureComponent<IListItemProps, IListItemState> {

  onItemClick = (item: IItemProps) => {
    const { onItemClick } = this.props;

    onItemClick && onItemClick(item);
  }

  render() {

    const { items, classNames, selectedItemId, onDeleteClick, onCreateNewClick, style } = this.props;
    
    return (
      <div className={`list-group${` ${classNames}` || undefined}`} style={style}>
        {
          !!onCreateNewClick && 
          <Item
            key={'new'}
            id={0}
            title={'[+] Create New'}
            isSelected={0 === selectedItemId}
            onClick={onCreateNewClick}
          />
        }
        {
          items.map((item: IItemProps) => {
            return (
              <Item
                key={item.id}
                id={item.id}
                title={item.title}
                isSelected={item.id === selectedItemId}
                disabled={item.disabled}
                classNames={item.classNames}
                onClick={() => { item.onClick && item.onClick(item); this.onItemClick(item); }}
                onDeleteClick={onDeleteClick}
              />
            )
          })
        }
      </div>
    );
  }
}

export default ListItems;