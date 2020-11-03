import React, { useEffect, useState } from 'react'
import { Route } from 'react-router-dom'
import { useStore } from '../../stores/StoreContext'
import Layout from '../shared/Layout'

function AuthorizeRoute(props: any): JSX.Element {
    const [ready, setReady] = useState(false)

    const { authStore } = useStore()

    useEffect(() => {
        async function getToken() {
            await authStore.getToken()
            setReady(true)
        }

        if (ready && !authStore.isAuthenticated) {
            window.location.href = '/Identity/Account/Login'
        } else if (!authStore.isAuthenticated) {
            getToken()
        }
    })

    if (!ready) {
        return <div></div>
    } else {
        const { component: Component, ...rest } = props
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
}

export default AuthorizeRoute
