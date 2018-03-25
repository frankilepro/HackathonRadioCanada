import PropTypes from 'prop-types';
import React from 'react';
import { StyleSheet, ViewPropTypes, Animated, View } from 'react-native';
import { Icon } from 'react-native-elements'

export default class CustomBubble extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            rated: false,
        };
    }

    handleUpvote = () => {
        this.setState({ rated: true });
        this.props.handleUpvote();
    };

    handleDownvote = () => {
        this.setState({ rated: true });
        this.props.handleDownvote();
    };

    handleDismiss = () => {
        this.setState({ rated: true });
    };

    render() {
        if (!this.state.rated && this.props.currentMessage.user._id === 1) {
            return (
                <View style={styles.container}>
                    <Icon
                        name="thumb-up"
                        accessibilityLabel="Aimer ce contenu"
                        size={30}
                        color="#4080ff"
                        onPress={this.handleUpvote}
                        containerStyle={styles.iconContainer}

                    />
                    <Icon
                        name="thumb-down"
                        accessibilityLabel="Détester ce contenu"
                        size={30}
                        color="#e74c3c"
                        onPress={this.handleDownvote}
                        containerStyle={styles.iconContainer}
                    />
                    <Icon
                        name="cancel"
                        accessibilityLabel="Rester neutre"
                        size={30}
                        color="gray"
                        onPress={this.handleDismiss}
                        containerStyle={styles.iconContainer}
                    />
                </View>
            );
        }
        return null;
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        flexDirection: "row",
    },
    iconContainer: {
        paddingTop: 10,
        paddingBottom: 10,
        paddingLeft: 10,
        paddingRight: 10,
    },
});

CustomBubble.defaultProps = {
    currentMessage: {},
    containerStyle: {},
    mapViewStyle: {},
};

CustomBubble.propTypes = {
    currentMessage: PropTypes.object,
    containerStyle: ViewPropTypes.style,
    mapViewStyle: ViewPropTypes.style,
};
