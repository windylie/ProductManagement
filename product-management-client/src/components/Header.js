import React from 'react';
import LoginMenu from './LoginMenu';
import { Link } from 'react-router-dom';

const Header = () => {
    return (
        <div className="ui secondary pointing menu">
            <Link to="/" className="item">Products</Link>
            <Link to="/product/manage" className="item">Manage Product</Link>
            <Link to="/product/create" className="item">Add New Product</Link>
            <Link to="/user/login" className="right floated item"><LoginMenu /></Link>
        </div>
    );
};

export default Header;
