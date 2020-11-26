import '../styles/application.scss';
import React from 'react';

const App = (props) => {
  return (
    <div className={props.className}>
      {props.children}
    </div>
  );
};

export default App;
