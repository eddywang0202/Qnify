import React from 'react';

interface IGridProps {
  rows: number,
  columns: number,
  skipRow?: number,
  hideRowIndices?: number[],
  cellClassName?: string,
  elements?: JSX.Element[]
}

const Grid = (props: IGridProps) => {

  let cells: JSX.Element[] = [];

  const { rows, cellClassName, skipRow, hideRowIndices } = props;

  let cellClassNames = 'col';
  if (cellClassName) cellClassNames += ` ${cellClassName}`;

  let startRow = skipRow || 0;
  let _hideRowsIndices = hideRowIndices || [];

  if (props.elements) {
    for (let i = startRow; i < props.rows; i++) {
      if (_hideRowsIndices.some(r => r === i)) continue;
      for(let j = 0; j < props.columns; j++) {
        let pos = (rows * i) + j + 1;
        cells.push(<div key={pos} className={cellClassNames}>{props.elements[pos-1]}</div>);
      }
      (i != props.rows - 1) && cells.push(<div key={Date.now()} className={'w-100'}></div>);
    }
  }

  return (
    <div className={'row grid'}>
      {
        cells.map(c => c)
      }
    </div>
  );
};

export default Grid;