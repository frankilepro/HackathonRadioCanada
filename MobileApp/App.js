import React from 'react';
import { View, StyleSheet, StatusBar } from "react-native";
import Chat from './components/Chat';
import Login from "./components/Login";

export default class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            logged: false,
            id: null
        }
    }

    handleLogin = (id = 42) => {
        this.setState({
            logged: true,
            id: id
        });
    };

    render() {
        let ElementToRender = <Login handleLogin={this.handleLogin}/>;
        if (this.state.logged) {
            ElementToRender = <Chat id={this.state.id}/>;
        }

        return (
            <View style={styles.container}>
                {ElementToRender}
            </View>
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
