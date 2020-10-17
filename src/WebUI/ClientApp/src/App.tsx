import React from 'react'
import Feed from './components/Feed'
import { BrowserRouter as Router, Switch } from 'react-router-dom'
import { CloudinaryContext } from 'cloudinary-react'
import Invite from './components/Invite'
import { store, StoreContext } from './stores/StoreContext'
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute'

function App() {
    const cloudName = process.env.REACT_APP_CLOUDINARY_CLOUD_NAME

    return (
        <CloudinaryContext cloudName={cloudName}>
            <StoreContext.Provider value={store}>
                <Router>
                    <Switch>
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
