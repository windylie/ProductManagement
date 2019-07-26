import React from 'react';

class Messages extends React.Component {
    getIcon(isSuccessful) {
        if (isSuccessful)
            return "check icon green";

        return "close icon red";
    }

    renderMessage(response)
    {
        const isSuccessful = response.status;
        const messageList = response.messages;

        if (!messageList || messageList.length === 0) {
            return <div></div>;
        }

        return messageList.map((message, index) => {
            return (
                <a className="item" key={index}>
                    <i className={this.getIcon(isSuccessful)} />
                    <div className="content">
                        <div className="description">{message}</div>
                    </div>
                </a>
            );
        });
    }

    render() {
        return(
            <div className="ui list">
                {this.renderMessage(this.props.response)}
            </div>
        );
    }
}

export default Messages;
