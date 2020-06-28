import React from 'react'
import { BrowserRouter as Router, Switch, Route, Link } from 'react-router-dom'
import logo from './logo.svg'
import './App.scss'
import Profile from './components/Profile'
import Messages from './components/Messages'
import Feed from './components/Feed'
import { Container } from 'react-bootstrap'
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute'
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants'
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes'

function App(): JSX.Element {
    return (
        <Router>
            <div className="App">
                <Container fluid>
                    <Switch>
                        <Route exact path="/">
                            <header>
                                <img src={logo} className="App-logo" alt="logo" />
                                <p>
                                    Edit <code>src/App.tsx</code> and save to reload.
                                </p>
                                <a
                                    className="App-link"
                                    href="https://reactjs.org"
                                    target="_blank"
                                    rel="noopener noreferrer"
                                >
                                    Learn React
                                </a>
                                <Link to="/profile">Profile</Link>
                            </header>
                        </Route>
                        <Route path="/profile" component={Profile} />
                        <Route path="/messages" component={Messages} />
                        <AuthorizeRoute path="/feed/:feedId" component={Feed} />
                        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
                    </Switch>
                </Container>
            </div>
        </Router>
    )
}

export default App
