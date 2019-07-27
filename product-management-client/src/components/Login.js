import React from 'react';
import Messages from './Messages';
import api from '../api/productManagementApi';

class Login extends React.Component {
    state = {
        username : '',
        password : '',
        response : {
            status : false,
            messages : []
        }}

    onUsernameChange = (event) => {
        this.setState({ username : event.target.value });
    }

    onPasswordChange = (event) => {
        this.setState({ password : event.target.value });
    }

    onLoginBtnClick = () => {
        const url = '/users/authenticate';
        api.post(url, {
            username : this.state.username,
            password : this.state.password
            }).then(res => {
                localStorage.setItem('token', res.data.token);
                window.location = '/';
            })
            .catch((error) => {
                this.setState({
                    response: {
                        status: error.response.data.isSuccessful,
                        messages: error.response.data.messages
                    }});
            });
    }

    onRegisterBtnClick = () => {
        const url = '/users';
        api.post(url, {
            username : this.state.username,
            password : this.state.password
            }).then(res => {
                this.setState({
                    response: {
                        status: res.data.isSuccessful,
                        messages: ['User is registered successfully! Please login!']
                    }});
            }).catch((error) => {
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
                    <label>Username</label>
                    <input type="text"
                           placeholder="Username"
                           value={this.state.username}
                           onChange={this.onUsernameChange} />
                </div>
                <div className="field">
                    <label>Password</label>
                    <input type="password"
                           value={this.state.password}
                           onChange={this.onPasswordChange} />
                </div>
                <div class="ui fluid buttons">
                    <button className="ui button" onClick={this.onLoginBtnClick}>Login</button>
                    <div className="or"></div>
                    <button className="ui positive button" onClick={this.onRegisterBtnClick}>Register</button>
                </div>
                <Messages response={this.state.response} />
            </div>
        );
    }
}

export default Login;
