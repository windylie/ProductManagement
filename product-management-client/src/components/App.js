import React from 'react';
import { BrowserRouter, Route } from 'react-router-dom';
import Header from './Header';
import ProductList from './ProductList';
import ProductManagement from './ProductManagement';
import CreateProduct from './CreateProduct';
import Login from './Login';

const App = () => {
    return (
        <div className="ui container">
            <BrowserRouter>
                <div>
                    <Header />
                    <Route path="/" exact component={ProductList} />
                    <Route path="/product/manage" component={ProductManagement} />
                    <Route path="/product/create" component={CreateProduct} />
                    <Route path="/user/login" component={Login} />
                </div>
            </BrowserRouter>
        </div>
    );
};

export default App;
