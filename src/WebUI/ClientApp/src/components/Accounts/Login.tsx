import React from 'react'
import { Button, Form } from 'react-bootstrap'
import { ILoginProps } from '../shared/Layout'

function Login({ setLoggedIn }: ILoginProps) {
    const [email, setEmail] = React.useState('')
    const [password, setPassword] = React.useState('')
    const [firstName, setFirstName] = React.useState('')
    const [lastName, setLastName] = React.useState('')
    const [loggingIn, setLoggingIn] = React.useState(true)
    const [loginError, setLoginError] = React.useState('')

    const registerWithEmail = () => {
        // Accounts.createUser(
        //     { email, password, profile: { firstName, lastName, name: `${firstName} ${lastName}` } },
        //     function (error) {
        //         if (!error) {
        //             setLoggedIn(true)
        //         }
        //         console.log(error)
        //     },
        // )
    }

    const loginWithEmail = () => {
        if (email && password) {
            // Meteor.loginWithPassword(email, password, (error) => {
            //     if (!error) {
            //         setLoggedIn(true)
            //     }
            //     console.log(error)
            //     setLoginError(error?.reason)
            // })
        }
    }

    const loginWithFacebook = () => {
        // Meteor.loginWithFacebook(
        //     {
        //         requestPermissions: ['public_profile', 'email'],
        //     },
        //     function (error) {
        //         if (!error) {
        //             setLoggedIn(true)
        //         }
        //         error?.message && setLoginError(error?.message)
        //     },
        // )
    }

    return (
        <Form className="w-25">
            <Form.Group>
                <Button className="w-100" variant="primary" onClick={loginWithFacebook}>
                    Login with Facebook
                </Button>
            </Form.Group>
            <span>---- OR ----</span>
            {!loggingIn && (
                <>
                    <Form.Group>
                        <input
                            style={{ width: '45%' }}
                            type="text"
                            placeholder="First Name"
                            onChange={(e) => setFirstName(e.target.value)}
                        ></input>
                        <input
                            className="float-right"
                            style={{ width: '45%' }}
                            type="text"
                            placeholder="Last Name"
                            onChange={(e) => setLastName(e.target.value)}
                        ></input>
                    </Form.Group>
                </>
            )}
            <Form.Group>
                <input
                    className="w-100"
                    type="email"
                    placeholder="Email"
                    onChange={(e) => setEmail(e.target.value)}
                ></input>
            </Form.Group>
            <Form.Group>
                <input
                    className="w-100"
                    type="password"
                    placeholder="Password"
                    onChange={(e) => setPassword(e.target.value)}
                ></input>
            </Form.Group>
            <Form.Group>
                <Button className="w-100" variant="primary" onClick={loggingIn ? loginWithEmail : registerWithEmail}>
                    {loggingIn ? 'Login' : 'Register'}
                </Button>
                <span className="ml-1">
                    or{' '}
                    <span className="fake-link" onClick={() => setLoggingIn(!loggingIn)}>
                        {loggingIn ? 'Create an account' : 'Already have an account?'}
                    </span>
                </span>
            </Form.Group>
            {loginError && <span className="error-message">{loginError}</span>}
        </Form>
    )
}

export default Login
