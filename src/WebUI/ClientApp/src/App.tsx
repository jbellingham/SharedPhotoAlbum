import React from 'react'
import Feed from './components/Feed'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import { CloudinaryContext } from 'cloudinary-react'
import Invite from './components/Invite'
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants'
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes'
import { store, StoreContext } from './stores/StoreContext'
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute'

function App() {
    const [cloudName] = React.useState()//Meteor.settings.public.cloudinary.cloudName)

    return (
        <CloudinaryContext cloudName={cloudName}>
            <StoreContext.Provider value={store}>
            <Router>
                <Switch>
                    <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
                    <AuthorizeRoute path="/invite/:inviteCode" component={Invite} />
                    <AuthorizeRoute path="/:feedId" component={Feed} />
                    <AuthorizeRoute path="/" component={Feed} />
                </Switch>
            </Router>
            </StoreContext.Provider>
        </CloudinaryContext>
    )
}

export default App
