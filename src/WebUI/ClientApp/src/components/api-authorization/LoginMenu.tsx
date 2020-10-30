import React, { Fragment } from 'react'
import { Button } from 'react-bootstrap'
import { NavItem, NavLink } from 'reactstrap'
import { Link } from 'react-router-dom'
import Cookies from 'js-cookie'
import { observer } from 'mobx-react'
import { useStore } from '../../stores/StoreContext'

const LoginMenu = observer(() => {
    const { userStore } = useStore()
    const { user } = userStore

    if (!user) {
        userStore.getUserProfile()
    }

    const logout = async () => {
        Cookies.remove('auth_token')
        const result = await fetch('/api/logout', {
            method: 'POST',
        })
        if (result.ok) {
            window.location.href = '/Identity/Account/Login'
        }
    }

    return (
        <Fragment>
            <NavItem>
                {/* <NavLink tag={Link} className="text-dark" to={profilePath}> */}
                <NavLink tag={Link} className="text-dark" to={'/'}>
                    Hello {user?.firstName}
                </NavLink>
            </NavItem>
            <NavItem>
                <Button variant="primary" onClick={logout}>
                    Logout
                </Button>
            </NavItem>
        </Fragment>
    )
    // }
})

export default LoginMenu
