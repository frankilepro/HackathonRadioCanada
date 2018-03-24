import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { GiftedChat, Bubble } from 'react-native-gifted-chat';
import VoiceModule from "./Voice";

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
        // location: {
        //   latitude: 48.864601,
        //   longitude: 2.398704
        // },
    }*/
];

export default class Chat extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            id: props.id,
            messages: [],
            loadEarlier: true,
            typingText: null,
            isLoadingEarlier: false,
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

    onLoadEarlier = () => {
        this.setState((previousState) => {
            return {
                isLoadingEarlier: true,
            };
        });

        if (this._isMounted === true) {
            this.setState((previousState) => {
                return {
                    messages: GiftedChat.prepend(previousState.messages, previous_messages),
                    loadEarlier: false,
                    isLoadingEarlier: false,
                };
            });
        }
    };

    onSend = (messages = []) => {
        this.setState((previousState) => {
            return {
                messages: GiftedChat.append(previousState.messages, messages),
            };
        });

        this.process(messages);
    };

    process = (messages) => {
        if (messages.length > 0) {
            if ((messages[0].image || messages[0].location) || !this._isAlright) {
                this.setState((previousState) => {
                    return {
                        typingText: 'The assistant is processing your request'
                    };
                });
            }
        }

        if (this._isMounted === true) {
            fetch('http://whatthecommit.com/index.txt',
                { method: 'get', mode: 'cors' })
                .then(response => {
                    response.text().then(text => {
                        messages[0].received = true;
                        this.onReceive(text);
                        this.setState({ typingText: null });
                    });
                }).catch((error) => {
                alert('Request failed', error);
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
                }),
            };
        });
    };

    renderCustomActions = (props) => {
        return (
            <VoiceModule/>
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
    }

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

    onLongPress = (context, message) => {
        if(message.user._id !== this.state.id){
            const options = [
                'Upvote',
                'Downvote',
                'Cancel'
            ];
            const cancelButtonIndex = options.length - 1;
            context.actionSheet().showActionSheetWithOptions({
                    options,
                    cancelButtonIndex,
                },
                (buttonIndex) => {
                    switch (buttonIndex) {
                        case 0:
                            alert("You liked it");
                            break;
                        case 1:
                            alert("You disliked it");
                            break;
                    }
                });
        }
    };

    render() {
        return (
            <GiftedChat
                messages={this.state.messages}
                onSend={this.onSend}
                loadEarlier={this.state.loadEarlier}
                onLoadEarlier={this.onLoadEarlier}
                isLoadingEarlier={this.state.isLoadingEarlier}

                user={{
                    _id: this.props.id,
                }}

                renderActions={this.renderCustomActions}
                renderBubble={this.renderBubble}
                renderFooter={this.renderFooter}
                onLongPress={this.onLongPress}
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

