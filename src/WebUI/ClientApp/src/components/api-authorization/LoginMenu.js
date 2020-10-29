import React, { Component, Fragment } from 'react'
import { Button } from 'react-bootstrap'
import { NavItem, NavLink } from 'reactstrap'
import { Link } from 'react-router-dom'
import authService from './AuthorizeService'
import { ApplicationPaths } from './ApiAuthorizationConstants'
import Cookies from 'js-cookie'

export class LoginMenu extends Component {
    constructor(props) {
        super(props)

        this.state = {
            isAuthenticated: false,
            userName: null,
        }
    }

    componentDidMount() {
        this._subscription = authService.subscribe(() => this.populateState())
        this.populateState()
    }

    componentWillUnmount() {
        authService.unsubscribe(this._subscription)
    }

    async populateState() {
        const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()])
        this.setState({
            isAuthenticated,
            userName: user && user.name,
        })
    }

    render() {
        const { isAuthenticated, userName } = this.state
        if (!isAuthenticated) {
            const registerPath = `${ApplicationPaths.Register}`
            const loginPath = `${ApplicationPaths.Login}`
            return this.anonymousView(registerPath, loginPath)
        } else {
            const profilePath = `${ApplicationPaths.Profile}`
            const logoutPath = { pathname: `${ApplicationPaths.LogOut}`, state: { local: true } }
            return this.authenticatedView(userName, profilePath, logoutPath)
        }
    }

    authenticatedView(userName, profilePath, logoutPath) {
        const logout = async () => {
            Cookies.remove('auth_token')
            const result = await fetch('/api/logout', {
                method: 'POST',
            })
            if (result.ok) {
                window.location = '/Identity/Account/Login'
            }
        }
        return (
            <Fragment>
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to={profilePath}>
                        Hello {userName}
                    </NavLink>
                </NavItem>
                <NavItem>
                    <Button variant="primary" onClick={logout}>
                        Logout
                    </Button>
                </NavItem>
            </Fragment>
        )
    }

    anonymousView(registerPath, loginPath) {
        return (
            <Fragment>
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to={registerPath}>
                        Register
                    </NavLink>
                </NavItem>
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to={loginPath}>
                        Login
                    </NavLink>
                </NavItem>
            </Fragment>
        )
    }
}
