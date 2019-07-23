import React from 'react';

class ProductFilter extends React.Component {
    state = { productList : [] }

    async componentDidMount() {
        // to do : enable when integrate with backend api
        // const response = await api.get('/products');
        // this.setState({ productList : response.data });
    }

    render() {
        return (
            <select className="ui dropdown" onChange={e => this.props.onProductSelected(e.target.value)}>
                <option key="title" value="0">Select product</option>
                {this.renderCustomerList()}
            </select>
        );
    }

    renderCustomerList() {
        return this.state.productList.map ((product) => {
            return (
                <option key={product.id} value={product.id}>{product.description}</option>
            );
        });
    }
}

export default ProductFilter;
