import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { FormLabel, FormInput } from 'react-native-elements'
import Button from "react-native-elements/src/buttons/Button";

export default class Login extends React.Component {
    render() {
        return (
            <View style={styles.container}>
                <FormLabel labelStyle={styles.form}>Nom d'utilisateur</FormLabel>
                <FormInput/>

                <FormLabel labelStyle={styles.form}>Mot de passe</FormLabel>
                <FormInput/>

                <Button
                    large
                    iconRight={{ name: 'person' }}
                    backgroundColor="#4080ff"
                    title="Se connecter"
                    onPress={this.props.handleLogin}
                />

                <Button
                    large
                    iconRight={{ name: 'done' }}
                    backgroundColor="#424242"
                    title="S'enregistrer"
                />
            </View>
        );
    }
}

const styles = StyleSheet.create({
        container: {
            flex: 1,
            marginTop: 100,
        },
    form: {
            color:'#424242'
    }
    }
);
