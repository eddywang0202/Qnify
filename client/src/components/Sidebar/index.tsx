import React from 'react';
import { NavLink, withRouter } from 'react-router-dom';
import Auth from '../Auth';
import { IUserIdentity } from '../Auth/types';
import { IRole } from '../Auth/types';

import companyLogo from '~/images/company_logo.png';

interface ISidebarProps {
  userIdentity: IUserIdentity
}

const Sidebar = (props: ISidebarProps) => {

  const logout = () => {
    Auth.resetAllToken();
    props.history.push('/');
  }

  const getNavLinks = (): any[] => {
    const { role } = props.userIdentity;
    switch (role) {
      case 'admin':
        return [
          <NavLink key={'dashboard'} activeClassName={'active'} className={'nav-link'} to={'/admin'} exact>Dashboard</NavLink>,
          <NavLink key={'managetest'} activeClassName={'active'} className={'nav-link'} to={'/managetest'} exact>Manage Tests</NavLink>,
          <NavLink key={'token'} activeClassName={'active'} className={'nav-link'} to={'/token'}>Manage Access Token</NavLink>,
          <NavLink key={'image'} activeClassName={'active'} className={'nav-link'} to={'/image'}>Manage Test Cases</NavLink>,
          <NavLink key={'consentMake'} activeClassName={'active'} className={'nav-link'} to={'/consent'}>Consent Document</NavLink>,
          <NavLink key={'report'} activeClassName={'active'} className={'nav-link'} to={'/report'}>View Reports</NavLink>,
        ];

      default:
        return [];
    }
  }

  if (props.userIdentity.role === 'admin') {
    return (
      <div className='sidebar'>
        {/* <div className='profile'>
          <img draggable={false} className='logo' src={companyLogo} />
        </div> */}
        <div>
          {getNavLinks && getNavLinks().map((navLink) => navLink)}
        </div>
        <div className='footer'>
          <button className={'btn-logout'} onClick={logout}>Logout</button>
        </div>
      </div>
    );
  }
  else if (props.userIdentity.role === 'member' || props.userIdentity.role === 'guest') {
    return (
      <div className='navbar sticky-top'>
        <div className='profile'>
          Welcome{/* Welcome {props.userIdentity.name} */}
        </div>
        <div className='footer'>
          <button className={'btn btn-light btn-logout'} onClick={logout}>Exit</button>
        </div>
      </div>
    );
  }
  else return (null)
};

export default withRouter(Sidebar);