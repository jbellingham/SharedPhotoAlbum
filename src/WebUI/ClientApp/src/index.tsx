import React from 'react'
import ReactDOM from 'react-dom'
import App from './App'
import './App.scss'
import * as serviceWorker from './serviceWorker'
import { ApolloClient } from 'apollo-client'
import { ApolloLink } from 'apollo-link'
import { HttpLink } from 'apollo-boost'
import { InMemoryCache } from 'apollo-cache-inmemory'
import { ApolloProvider } from 'react-apollo'

const client = new ApolloClient({
    link: ApolloLink.from([
        // new MeteorAccountsLink(),
        new HttpLink({
            uri: '/graphql',
        }),
    ]),
    cache: new InMemoryCache(),
})

ReactDOM.render(
    <ApolloProvider client={client}>
        <App />
    </ApolloProvider>,
    document.getElementById('root'),
)

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister()
