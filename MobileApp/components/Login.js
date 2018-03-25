import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { FormLabel, FormInput } from 'react-native-elements'
import Button from "react-native-elements/src/buttons/Button";

export default class Login extends React.Component {
    render() {
        return (
            <View style={styles.container}>
                <FormLabel>Nom d'utilisateur</FormLabel>
                <FormInput/>

                <FormLabel>Mot de passe</FormLabel>
                <FormInput/>

                <Button
                    large
                    iconRight={{ name: 'person' }}
                    backgroundColor="#4080ff"
                    title='Login'
                    onPress={this.props.handleLogin}
                />

                <Button
                    large
                    iconRight={{ name: 'done' }}
                    title='Register'
                />
            </View>
        );
    }
}

const styles = StyleSheet.create({
        container: {
            flex: 1,
        }
    }
);
