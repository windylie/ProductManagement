import React from 'react';
import Messages from './Messages';
import api from '../api/productManagementApi';

class CreateProduct extends React.Component {
    state = {
        description : '',
        model : '',
        brand : '',
        response : {
            status: false,
            messages: []
        }}

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
        const url = '/products';
        api.post(url, {
            description : this.state.description,
            model : this.state.model,
            brand : this.state.brand
            }).then(res => {
                this.setState({
                    response: {
                        status: res.data.isSuccessful,
                        messages: ['Product is added successfully!']
                    }});
            })
            .catch((error) => {
                this.setState({
                    response: {
                        status: error.response.data.isSuccessful,
                        messages: error.response.data.messages
                    }});
            });
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
                <button className="ui fluid button" onClick={this.onBtnClick}>Add Product</button>
                <Messages response={this.state.response} />
            </div>
        );
    }
}

export default CreateProduct;
