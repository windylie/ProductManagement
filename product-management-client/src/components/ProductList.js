import React from 'react';
import Loading from './Loading';
import api from '../api/productManagementApi';

class ProductList extends React.Component {
    state = { productList : [], loading : true };

    renderResultTable()
    {
        return (
            <div>
                <table className="ui celled table">
                    <thead>
                        <tr>
                            <th>Description</th>
                            <th>Model</th>
                            <th>Brand</th>
                        </tr>
                    </thead>
                    <tbody>
                        { this.renderTableBody() }
                    </tbody>
                </table>
            </div>
        );
    }

    renderTableBody()
    {
        const { productList } = this.state;

        if (!productList || productList.length === 0) {
            return (
                <tr>
                    <td colSpan="3" className="center aligned">
                        No records found
                    </td>
                </tr>);
        }

        return productList.map((product) => {
            return (
                <tr key={product.id }>
                  <td data-label="Description">{product.description}</td>
                  <td data-label="Model">{product.model}</td>
                  <td data-label="Brand">{product.brand}</td>
                </tr>
            );
        });
    }

    async componentDidMount() {
        const response = await api.get('/products');
        this.setState({ productList : response.data, loading : false });
    }

    render() {
        return this.state.loading ? <Loading /> : this.renderResultTable();
    }
}

export default ProductList;
