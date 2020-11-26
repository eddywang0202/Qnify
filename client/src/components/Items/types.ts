import { CSSProperties } from "react";

export interface IItemProps {
  id: number,
  title: string,
  order?: number,
  isSelected?: boolean,
  disabled?: boolean,
  classNames?: string,
  onClick?: (item: IItemProps) => void,
  onDeleteClick?: (e: React.MouseEvent, item: IItemProps) => void
}

export interface IListItemProps {
  items: IItemProps[],
  selectedItemId?: number,
  classNames?: string,
  style?: CSSProperties,
  onItemClick?: (item: IItemProps) => void,
  onDeleteClick?: (e: React.MouseEvent, item: IItemProps) => void,
  onCreateNewClick?: () => void
}

export interface IListItemState {
  selectedItemId: number
}