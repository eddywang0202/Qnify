const findLastIndex = (array: [], searchKey: string, searchValue: any) => {
  var index = array.slice().reverse().findIndex(x => x[searchKey] === searchValue);
  var count = array.length - 1
  var finalIndex = index >= 0 ? count - index : index;
  return finalIndex;
}

export { findLastIndex };