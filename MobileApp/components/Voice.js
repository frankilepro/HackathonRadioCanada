import React from 'react';
import { Icon } from 'react-native-elements'
import Voice from 'react-native-voice';

export default class VoiceRecognition extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            recoding: false,
            color: 'gray',
            recognized: false,
            pitch: false,
            error: false,
            end: false,
            started: false,
            results: [],
            partialResults: []
        };

        Voice.onSpeechStart = this.onSpeechStart.bind(this);
        Voice.onSpeechRecognized = this.onSpeechRecognized.bind(this);
        Voice.onSpeechEnd = this.onSpeechEnd.bind(this);
        Voice.onSpeechError = this.onSpeechError.bind(this);
        Voice.onSpeechResults = this.onSpeechResults.bind(this);
        Voice.onSpeechPartialResults = this.onSpeechPartialResults.bind(this);
        Voice.onSpeechVolumeChanged = this.onSpeechVolumeChanged.bind(this);
    }

    componentWillUnmount() {
        Voice.destroy().then(Voice.removeAllListeners);
    }

    onSpeechStart(e) {
        this.setState({
            started: true,
        });
    }

    onSpeechRecognized(e) {
        this.setState({
            recognized: true,
        });
    }

    onSpeechEnd(e) {
        this.setState({
            end: true
        });
    }

    onSpeechError(e) {
        this.setState({
            error: JSON.stringify(e.error),
        });
    }

    onSpeechResults(e) {
        this.setState({
            results: e.value,
        });
        this.state.results.map((result, index) => {
           alert(result);
        });
    }

    onSpeechPartialResults(e) {
        this.setState({
            partialResults: e.value,
        });
    }

    onSpeechVolumeChanged(e) {
        this.setState({
            pitch: e.value,
        });
    }

    async _startRecognizing(e) {
        this.setState({
            recording: true,
            recognized: false,
            pitch: false,
            error: false,
            started: false,
            results: [],
            partialResults: [],
            end: false,
            color: 'red'
        });
        try {
            await Voice.start('fr-CA');
        } catch (e) {
            console.error(e);
        }
    }

    async _stopRecognizing(e) {
        this.setState({
            recording: false,
            color: 'gray'
        });

        try {
            await Voice.stop();
        } catch (e) {
            console.error(e);
        }
    }

    async _cancelRecognizing(e) {
        try {
            await Voice.cancel();
        } catch (e) {
            console.error(e);
        }
    }

    async _destroyRecognizer(e) {
        try {
            await Voice.destroy();
        } catch (e) {
            console.error(e);
        }
        this.setState({
            recognized: false,
            pitch: false,
            error: false,
            started: false,
            results: [],
            partialResults: [],
            end: false
        });
    }

    toggleRecord = () => {
        if (this.state.recording) {
            this._stopRecognizing().catch(() => alert("Error stopping"));
        } else {
            if(!Voice.isAvailable()){
                alert("Le service de reconnaissance vocale n'est disponible dans votre système.");
                return;
            }
            this._startRecognizing().catch(() => alert("Error starting"));
        }

    };

    render() {
        return (
            <Icon
                name="mic"
                color={this.state.color}
                onPress={this.toggleRecord}
            />
        );
    }
}
