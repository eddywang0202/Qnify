import React from 'react';

import { BrowserRouter, Redirect } from 'react-router-dom';

const NoAccess = (props) => {
  // return (
  //   <Redirect to={'/'} />
  //   <div style={{ backgroundColor: 'rgb(29, 29, 29)', width: '100%', height: '100%', textAlign: 'center', paddingTop: '25px'}}>
  //     <h1>No Access</h1>
  //     <p>You do not have access to this page.</p>
  //     <button className='btn btn-primary' onClick={props.history.goBack}>Go Back</button>
  //   </div>
  // );

  return <Redirect to={'/'} />
};

export default NoAccess;