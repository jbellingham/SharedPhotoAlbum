import React from 'react'
import { Component } from 'react'
import { Route, Redirect } from 'react-router-dom'
import Layout from './Layout'

function AuthorizedRoute(props) {
    const [isLoggedIn, setIsLoggedIn] = React.useState(false)
    const { component: Component, ...rest } = props
    // useTracker(() => {
    //     const userId = null//Meteor.userId()
    //     setIsLoggedIn(!!userId)
    // }, [])

    return (
        <Route
            {...rest}
            render={(props) => {
                return (
                    <Layout>
                        <Component {...props} />
                    </Layout>
                )
            }}
        />
    )
}

export default AuthorizedRoute
