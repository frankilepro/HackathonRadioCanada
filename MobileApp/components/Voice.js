import React from 'react';
import { Icon } from 'react-native-elements'
import Tts from 'react-native-tts';
import Voice from 'react-native-voice';

export default class VoiceModule extends React.Component {
    constructor(props){
        super(props);
        this.state = {
            isActive : false,
            color: 'gray'
        }
    }

    handlePress = () => {
        if(this.state.isActive){
            this.setState({
                isActive : false,
                color: 'gray'
            });

        } else {
            this.setState({
                isActive : true,
                color: 'red'
            });
        }

    };

    render() {
        return (
            <Icon
                name="mic"
                onPress={this.handlePress}
            />
        );
    }
}
