import React from 'react'
import { NavMenu } from './NavMenu'
// import Login from '../Accounts/Login'
import CollapseMenu from './CollapseMenu'

export interface ILoginProps {
    setLoggedIn: (value: boolean) => void
}

function Layout(props: any) {
    return (
        <>
            {/* <CollapseMenu></CollapseMenu> */}
            <NavMenu />
            <div className="App">
                <CollapseMenu></CollapseMenu>
                {props.children}
            </div>
        </>
    )
}

export default Layout
