import React, { Component } from 'react';
import { connect } from 'react-redux';

import { IDashboardState } from './types';

function mapStateToProps(state) {
  return {
    title: state.dashboard.title
  };
}

class Dashboard extends Component<IDashboardState> {
  render() {
    return (
      <div>
        <h1>{this.props.title}</h1>
        <p>Questionnaire tool for everything.</p>
      </div>
    );
  }
}

export default connect(
  mapStateToProps,
)(Dashboard);