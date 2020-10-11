import React from 'react'
import { Form } from 'react-bootstrap'
import authService from './AuthorizeService'

function Login(): JSX.Element {
    const [email, setEmail] = React.useState()
    const [password, setPassword] = React.useState()

    const login = (e: MouseEvent) => {
        // authenticationService.login(username, password).then((user) => {
        //     this.props.history.push(from)
        // })
    }

    const loginWithFacebook = async (e: React.MouseEvent) => {
        e.preventDefault()
        await authService.signIn({ returnUrl: '/' })
    }

    return (
        <div>
            <h2>Login</h2>

            <Form>
                <div className="form-group">
                    <label htmlFor="username">Username</label>
                    <Form.Control name="username" type="text" value={email} />
                </div>
                <div className="form-group">
                    <label htmlFor="password">Password</label>
                    <Form.Control name="password" type="password" value={password} />
                </div>
                <div className="form-group">
                    <button className="btn btn-primary" onClick={login}>
                        Login
                    </button>
                    <button className="btn btn-primary" onClick={loginWithFacebook}>
                        Login with facebook
                    </button>
                </div>
            </Form>
        </div>
    )
}

export default Login
