import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { GiftedChat, Bubble } from 'react-native-gifted-chat';
import VoiceRecognition from "./Voice";
import CustomBubble from './CustomBubble';

previous_messages = [
    /*
    {
        _id: Math.round(Math.random() * 1000000),
        text: 'Yes, and I use Gifted Chat!',
        createdAt: new Date(Date.UTC(2016, 7, 30, 17, 20, 0)),
        user: {
            _id: 1,
            name: 'Developer',
        },
        sent: true,
        received: true,
    }*/
];

export default class Chat extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            id: props.id,
            messages: [],
            typingText: null,
        };

        this._isMounted = false;
        this._isAlright = null;
    }

    componentWillMount() {
        this._isMounted = true;
        this.setState(() => {
            return {
                // put old messages here
                messages: []
            };
        });
    }

    componentWillUnmount() {
        this._isMounted = false;
    }

    onSend = (messages = []) => {
        this.setState((previousState) => {
            return {
                messages: GiftedChat.append(previousState.messages, messages),
            };
        });

        this.process(messages);
    };

    handleUpvote = () => {
        alert("up");
    };

    handleDownvote = () => {
        alert("down");
    };

    process = (messages) => {
        if (messages.length > 0) {
            if ((messages[0].image || messages[0].location) || !this._isAlright) {
                this.setState((previousState) => {
                    return {
                        typingText: "Je traite votre requête..."
                    };
                });
            }
        }

        if (this._isMounted === true) {
            fetch('http://newsassistants.net/api/message/text/' + messages[0].text,
                { method: 'get', mode: 'cors' })
                .then(response => {
                    response.text().then(text => {
                        messages[0].received = true;

                        this.onReceive(text);
                        this.setState({ typingText: null });
                    });
                }).catch((error) => {
                this.setState({
                    typingText: "Je n'ai pas réussi à me connecter au serveur :("
                });
            });
        }
    };

    onReceive = (text) => {
        this.setState((previousState) => {
            return {
                messages: GiftedChat.append(previousState.messages, {
                    _id: Math.round(Math.random() * 1000000),
                    text: text,
                    createdAt: new Date(),
                    user: {
                        _id: 1,
                        name: 'News Assistant',
                        avatar: 'https://pbs.twimg.com/profile_images/958444996176232448/YhMYOu4R_400x400.jpg',
                    },
                    rated: false,
                }),
            };
        });
    };

    handleVoice = (text) => {
        let message = {
            _id: Math.round(Math.random() * 1000000),
            text: text,
            createdAt: new Date(),
            user: {
                _id: this.props.id,
            },
        };

        this.onSend([message]);
    };

    renderCustomActions = (props) => {
        return (
            <VoiceRecognition
                handleSendVoice={this.handleVoice}
            />
        );
    };

    renderCustomView = (props) => {
        return (
            <CustomBubble
                {...props}
                handleUpvote={this.handleUpvote}
                handleDownvote={this.handleDownvote}
            />
        );
    };

    renderBubble = (props) => {
        return (
            <Bubble
                {...props}
                wrapperStyle={{
                    left: {
                        backgroundColor: '#ffffff',
                    }
                }}
            />
        );
    };

    renderFooter = (props) => {
        if (this.state.typingText) {
            return (
                <View style={styles.footerContainer}>
                    <Text style={styles.footerText}>
                        {this.state.typingText}
                    </Text>
                </View>
            );
        }
        return null;
    };

    render() {
        return (
            <GiftedChat
                messages={this.state.messages}
                onSend={this.onSend}

                user={{
                    _id: this.props.id,
                }}

                renderActions={this.renderCustomActions}
                renderBubble={this.renderBubble}
                renderFooter={this.renderFooter}
                renderCustomView={this.renderCustomView}
                textInputProps={{
                    placeholder: "Parlez moi"
                }}
            />
        );
    }
}

const styles = StyleSheet.create({
    footerContainer: {
        marginTop: 10,
        marginLeft: 10,
        marginRight: 10,
        marginBottom: 10,
    },
    footerText: {
        fontSize: 14,
        color: '#aaa',
    },
});

