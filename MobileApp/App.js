import React from 'react';
import { KeyboardAvoidingView, StyleSheet } from "react-native";
import Chat from './components/Chat';
import Login from "./components/Login";

export default class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            logged: true,
            id: null
        }
    }

    handleLogin = (id) => {
        this.setState({
            logged: true,
            id: id
        });
    };

    render() {
        let ElementToRender = <Login/>;
        if (this.state.logged) {
            ElementToRender = <Chat id={this.state.id}/>;
        }
        return (
            <KeyboardAvoidingView style={styles.container} behavior="padding">
                {ElementToRender}
            </KeyboardAvoidingView>
        );
    }
}

const styles = StyleSheet.create({
        container: {
            flex: 1,
            backgroundColor: '#ecf0f1'
        }
    }
);
