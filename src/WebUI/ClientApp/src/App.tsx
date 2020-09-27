import React from 'react'
import Feed from './components/Feed'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import AuthorizedRoute from './components/shared/AuthorizedRoute'
import { CloudinaryContext } from 'cloudinary-react'
import Invite from './components/Invite'
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants'
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes'
import { store, StoreContext } from './stores/StoreContext'

function App() {
    const [cloudName] = React.useState()//Meteor.settings.public.cloudinary.cloudName)

    return (
        <CloudinaryContext cloudName={cloudName}>
            <StoreContext.Provider value={store}>
            <Router>
                <Switch>
                    <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
                    <AuthorizedRoute path="/invite/:inviteCode" component={Invite} />
                    <AuthorizedRoute path="/:feedId" component={Feed} />
                    <AuthorizedRoute path="/" component={Feed} />
                </Switch>
            </Router>
            </StoreContext.Provider>
        </CloudinaryContext>
    )
}

export default App
