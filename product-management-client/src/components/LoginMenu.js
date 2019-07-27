import React from 'react';

class LoginMenu extends React.Component {
    state = { isSignedIn : false }

    componentDidMount() {
        const token = localStorage.getItem('token');
        if (!token) {
            this.setState({ isSignedIn : false })
        } else {
            this.setState({ isSignedIn : true });
        }
    }

    onBtnClick = () => {
        if (this.state.isSignedIn) {
            this.setState({ isSignedIn : false })
            localStorage.removeItem('token');
        }
    }

    renderLoginText() {
        if (this.state.isSignedIn)
            return 'Logout';

        return 'Login';
    }

    render() {
        return (
            <button className="ui basic button" onClick={this.onBtnClick}>
                <i className="icon user"></i>
                {this.renderLoginText()}
            </button>
    )}
}

export default LoginMenu;
