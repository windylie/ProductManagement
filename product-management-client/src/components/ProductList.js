import React from 'react';
import Loading from './Loading';
import api from '../api/productManagementApi';

class ProductList extends React.Component {
    state = {
        description : '',
        model : '',
        brand : '',
        productList : [],
        loading : true };

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
                <tr key={product.id}>
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

    onBtnFilterClick = async () => {
        this.setState({ loading: true });
        var params = "descr=" + this.state.description + "&model=" + this.state.model + "&brand=" + this.state.brand;
        const response = await api.get('/products?' + params);
        this.setState({ productList : response.data, loading : false });
    }

    onDescriptionChange = (event) => {
        this.setState({ description: event.target.value });
    }

    onModelChange = (event) => {
        this.setState({ model: event.target.value });
    }

    onBrandChange = (event) => {
        this.setState({ brand: event.target.value });
    }

    renderBody()
    {
        if (this.state.loading)
            return <Loading />

        return (
            <div>
                <div className="ui mini form">
                    <div className="one field">
                        <div className="field">
                            <input placeholder="Description"
                                   type="text"
                                   value={this.state.description}
                                   onChange={this.onDescriptionChange} />
                        </div>
                    </div>
                    <div className="two fields">
                        <div className="field">
                            <input placeholder="Model"
                                   type="text"
                                   value={this.state.model}
                                   onChange={this.onModelChange} />
                        </div>
                        <div className="field">
                            <input placeholder="Brand"
                                   type="text"
                                   value={this.state.brand}
                                   onChange={this.onBrandChange} />
                        </div>
                    </div>
                    <div className="ui fluid submit button"
                         onClick={this.onBtnFilterClick}>
                             Filter
                    </div>
                </div>
                <div className="ui divider"></div>
                { this.renderResultTable() }
            </div>
        );
    }

    render() {
        return this.renderBody();
    }
}

export default ProductList;
