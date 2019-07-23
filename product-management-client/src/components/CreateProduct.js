import React from 'react';

class CreateProduct extends React.Component {
    state = {
        description : '',
        model : '',
        brand : '',
        response : ''}

    onDescriptionChange = (event) => {
        this.setState({ description : event.target.value });
    }

    onModelChange = (event) => {
        this.setState({ model : event.target.value });
    }

    onBrandChange = (event) => {
        this.setState({ brand : event.target.value });
    }

    onBtnClick = () => {
        // to do : enable when integrate with backend api
        // const url = '/products';
        // api.post(url, {
        //     description : this.state.phoneNo,
        //     model : this.state.model,
        //     brand : this.state.brand
        //     }).then(res => {
        //         this.setState({ response: 'Successfully added new product!'});
        //     })
        //     .catch((error) => {
        //         this.setState({ response: error.response.data.message });
        //     });
    }

    render() {
        return (
            <div className="ui form">
                <div className="field">
                    <label>Description</label>
                    <input type="text"
                           placeholder="Description"
                           value={this.state.description}
                           onChange={this.onDescriptionChange} />
                </div>
                <div className="field">
                    <label>Model</label>
                    <input type="text"
                           placeholder="Model"
                           value={this.state.model}
                           onChange={this.onModelChange} />
                </div>
                <div className="field">
                    <label>Brand</label>
                    <input type="text"
                           placeholder="Brand"
                           value={this.state.brand}
                           onChange={this.onBrandChange} />
                </div>
                <button className="ui button" onClick={this.onBtnClick}>Add Product</button>
                <div>{this.state.response}</div>
            </div>
        );
    }
}

export default CreateProduct;
