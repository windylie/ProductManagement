import React from 'react';
import ProductFilter from './ProductFilter';
import EditProduct from './EditProduct';

const SHOW_MODE = 'SHOW';
const EDIT_MODE = 'EDIT';

class ProductManagement extends React.Component {
    state = { productList : [], productId : 0, response : '', mode:SHOW_MODE }

    onInputChange = async (productId) => {
        // to do : enable when integrate with backend api
        // const response = await api.get('/products/' + productId);
        // this.setState({
        //     productList : response.data,
        //     productId : productId,
        //     response : '' });
    }

    onDeleteBtnClick = async (productId) => {
        // const url = '/products' + productId;
        // await api.delete(url)
        //     .then(res => {
        //         this.onInputChange(this.state.productId);
        //     })
        //     .catch((error) => {
        //         this.setState({ response: error.response.data.message });
        //     });
    }

    onEditBtnClick = () => {
        this.setState({mode: EDIT_MODE});
    }

    renderProductDetail() {
        //const productList = this.state.phoneList;
        const productList = [{id:"PR001",description:"aaa",model:"bbb",brand:"ccc"}];

        if (!productList || productList.length === 0) {
            return (
                <tr>
                    <td colSpan="5">
                        No records found
                    </td>
                </tr>);
        }

        return productList.map ((product) => {
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
                        <td colspan="2">
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
        });
    }

    renderUI()
    {
        if (this.state.mode === SHOW_MODE)  {
            return(
                <table class="ui very basic table">
                    {this.renderProductDetail()}
                </table>
            );
        } else {
            return <EditProduct />
        }

    }

    render() {
        return (
            <div className="ui form">
                <div className="field">
                    <ProductFilter onProductSelected={this.onInputChange} />
                </div>
                {this.renderUI()}
                <div>{this.state.message}</div>
            </div>
        );
    };
}

export default ProductManagement;
