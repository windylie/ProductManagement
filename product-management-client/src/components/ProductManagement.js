import React from 'react';
import ProductFilter from './ProductFilter';
import EditProduct from './EditProduct';
import Messages from './Messages';
import api from '../api/productManagementApi';

const SHOW_MODE = 'SHOW';
const EDIT_MODE = 'EDIT';

class ProductManagement extends React.Component {
    state = {
        productList : [],
        response : {status: false, messages: []},
        mode:SHOW_MODE,
        selectedProduct: null }

    async componentDidMount() {
        await this.getProductList();
    }

    getProductList = async() => {
        const response = await api.get('/products');
        this.setState({
            productList : response.data,
            selectedProduct: null
        });
    }

    onInputChange = async (productId) => {
        const response = await api.get('/products/' + productId);
        this.setState({
            selectedProduct : response.data,
            response : ''
        });
    }

    onDeleteBtnClick = async () => {
        const url = '/products/' + this.state.selectedProduct.id;
        await api.delete(url)
            .then(async (res) => {
                await this.getProductList();
                this.setState({
                    response: {
                        status: res.data.isSuccessful,
                        messages: ['Product is deleted successfully!']
                    }
                });
            })
            .catch((error) => {
                this.setState({
                    response: {
                        status: error.response.data.isSuccessful,
                        messages: error.response.data.messages
                    }});
            });
    }

    onEditBtnClick = () => {
        this.setState({mode: EDIT_MODE, response: ''});
    }

    onUpdated = async (response, productId) => {
        await this.onInputChange(productId);
        this.setState({mode: SHOW_MODE, response: response});
    }

    renderProductDetail() {
        const product = this.state.selectedProduct;

        if (!product) {
            return (
                <tbody>
                    <tr>
                        <td colSpan="2">
                            No records found
                        </td>
                    </tr>
                </tbody>);
        }

        return (
            <tbody>
                <tr>
                    <td className="collapsing">Description</td>
                    <td>{product.description}</td>
                </tr>
                <tr>
                    <td className="collapsing">Model</td>
                    <td>{product.model}</td>
                </tr>
                <tr>
                    <td className="collapsing">Brand</td>
                    <td>{product.brand}</td>
                </tr>
                <tr>
                    <td colSpan="2">
                        <button className="ui right floated icon button" onClick={this.onDeleteBtnClick}>
                            <i className="trash alternate icon" />
                        </button>
                        <button className="ui right floated icon button" onClick={this.onEditBtnClick}>
                            <i className="pencil alternate icon" />
                        </button>
                    </td>
                </tr>
            </tbody>
        );

    }

    renderUI() {
        if (this.state.mode === SHOW_MODE)  {
            return(
                <table className="ui very basic table">
                    {this.renderProductDetail()}
                </table>
            );
        } else {
            return <EditProduct product={this.state.selectedProduct}
                                onUpdated={this.onUpdated} />
        }

    }

    render() {
        return (
            <div className="ui form">
                <div className="field">
                    <ProductFilter productList={this.state.productList}
                                   onProductSelected={this.onInputChange} />
                </div>
                {this.renderUI()}
                <Messages response={this.state.response} />
            </div>
        );
    };
}

export default ProductManagement;
