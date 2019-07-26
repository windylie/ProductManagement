import React from 'react';

class ProductFilter extends React.Component {
    render() {
        return (
            <select className="ui dropdown" onChange={e => this.props.onProductSelected(e.target.value)}>
                <option key="title" value="0">Select product</option>
                {this.renderCustomerList()}
            </select>
        );
    }

    renderCustomerList() {
        return this.props.productList.map ((product) => {
            return (
                <option key={product.id} value={product.id}>{product.description}</option>
            );
        });
    }
}

export default ProductFilter;
