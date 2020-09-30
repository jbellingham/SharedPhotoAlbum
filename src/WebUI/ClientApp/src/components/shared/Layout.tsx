import React from 'react'
import { Container } from 'react-bootstrap'
import { NavMenu } from '../NavMenu'
// import Login from '../Accounts/Login'
import CollapseContainer from './CollapseContainer'

export interface ILoginProps {
    setLoggedIn: (value: boolean) => void
}

function Layout(props: any) {

    return (
        <>
            <CollapseContainer />
            <NavMenu />
            <div className="App">
                {/* {loggedIn ? ( */}
                   { props.children }
                {/* ) : ( */}
                    {/* <div className="vertical-center justify-content-center">
                        <Login setLoggedIn={setLoggedIn} />
                    </div>
                )} */}
            </div>
        </>
    )
}

export default Layout
