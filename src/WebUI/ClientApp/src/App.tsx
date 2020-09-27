import React from 'react'
import Feed from './components/Feed'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import Layout from './components/shared/Layout'
import AuthorizedRoute from './components/shared/AuthorizedRoute'
import { CloudinaryContext } from 'cloudinary-react'
import Invite from './components/Invite'

function App() {
    const [cloudName] = React.useState()//Meteor.settings.public.cloudinary.cloudName)

    return (
        <CloudinaryContext cloudName={cloudName}>
            <Router>
                <Switch>
                    <AuthorizedRoute path="/invite/:inviteCode" component={Invite} />
                    <AuthorizedRoute path="/:feedId" component={Feed} />
                    <AuthorizedRoute path="/" component={Feed} />
                </Switch>
            </Router>
        </CloudinaryContext>
    )
}

export default App
