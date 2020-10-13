import { Profile, User, UserManager, WebStorageStateStore } from 'oidc-client'
import { ApplicationPaths, ApplicationName } from './ApiAuthorizationConstants'
import Cookies from 'js-cookie'

export const AuthenticationResultStatus = {
    Redirect: 'redirect',
    Success: 'success',
    Fail: 'fail',
}

export class AuthorizeService {
    constructor() {
        window.fbAsyncInit = function () {
            window.FB.init({
                appId: '1259536097712653',
                cookie: true,
                xfbml: true,
                version: 'v2.8',
            })
            window.FB.AppEvents.logPageView()
            // eslint-disable-next-line no-restricted-globals
            window.FB.Event.subscribe('auth.statusChange', self.onStatusChange.bind(self))
        }.bind(this)

        // Load the SDK asynchronously
        ;(function (d, s, id) {
            let js,
                fjs = d.getElementsByTagName(s)[0]
            if (d.getElementById(id)) return
            js = d.createElement(s)
            js.id = id
            js.src = 'https://connect.facebook.net/en_US/sdk.js'
            fjs.parentNode.insertBefore(js, fjs)
        })(document, 'script', 'facebook-jssdk')
    }
    _callbacks = []
    _nextSubscriptionId = 0
    _user = null
    _isAuthenticated = false

    // By default pop ups are disabled because they don't work properly on Edge.
    // If you want to enable pop up authentication simply set this flag to false.
    _popUpDisabled = true

    isAuthenticated() {
        return !!Cookies.get('auth_token')
    }

    async getUser() {
        return Promise.resolve()
    }

    async onStatusChange(response) {
        if (response.status === 'connected') {
            await this.signIn()
        }
    }

    // async tokenExpired() {
    //     const user = await this.userManager?.getUser()
    //     return user.expired
    // }

    // async getAccessToken() {
    //     const result = await fetch('/api/token')
    //     const token = await result.text()
    //     Cookies.set('token', token)
    // }

    // We try to authenticate the user in three different ways:
    // 1) We try to see if we can authenticate the user silently. This happens
    //    when the user is already logged in on the IdP and is done using a hidden iframe
    //    on the client.
    // 2) We try to authenticate the user using a PopUp Window. This might fail if there is a
    //    Pop-Up blocker or the user has disabled PopUps.
    // 3) If the two methods above fail, we redirect the browser to the IdP to perform a traditional
    //    redirect flow.
    async signIn(state) {
        // await this.ensureUserManagerInitialized()
        try {
            // const silentUser = await this.userManager.signinSilent(this.createArguments())
            const data = {
                returnUrl: state.returnUrl,
            }
            const result = await fetch('/api/externallogin', {
                mode: 'no-cors',
                method: 'POST',
                body: JSON.stringify(data),
            })
            if (result.ok) {
                const tokenResponse = await result.json()
                Cookies.set('auth_token', tokenResponse.tokenString)
                this.updateState()
                return this.success()
            } else {
                // return this.error()
                throw new Error('poop')
            }
        } catch (silentError) {
            // User might not be authenticated, fallback to popup authentication
            console.log('Silent authentication error: ', silentError)

            try {
                // if (this._popUpDisabled) {
                //     throw new Error(
                //         "Popup disabled. Change 'AuthorizeService.js:AuthorizeService._popupDisabled' to false to enable it.",
                //     )
                // }

                // const popUpUser = await this.userManager.signinPopup(this.createArguments())
                // this.updateState(popUpUser)
                window.FB.login(
                    function (response) {
                        console.log(response)
                    },
                    { scope: 'public_profile,email' },
                )
                return this.success(state)
            } catch (popUpError) {
                if (popUpError.message === 'Popup window closed') {
                    // The user explicitly cancelled the login action by closing an opened popup.
                    return this.error('The user closed the window.')
                } else if (!this._popUpDisabled) {
                    console.log('Popup authentication error: ', popUpError)
                }

                // PopUps might be blocked by the user, fallback to redirect
                try {
                    await this.userManager.signinRedirect(this.createArguments(state))
                    return this.redirect()
                } catch (redirectError) {
                    console.log('Redirect authentication error: ', redirectError)
                    return this.error(redirectError)
                }
            }
        }
    }

    async completeSignIn(url) {
        try {
            await this.ensureUserManagerInitialized()
            const user = await this.userManager.signinCallback(url)
            this.updateState(user)
            return this.success(user && user.state)
        } catch (error) {
            console.log('There was an error signing in: ', error)
            return this.error('There was an error signing in.')
        }
    }

    // We try to sign out the user in two different ways:
    // 1) We try to do a sign-out using a PopUp Window. This might fail if there is a
    //    Pop-Up blocker or the user has disabled PopUps.
    // 2) If the method above fails, we redirect the browser to the IdP to perform a traditional
    //    post logout redirect flow.
    async signOut(state) {
        await this.ensureUserManagerInitialized()
        try {
            if (this._popUpDisabled) {
                throw new Error(
                    "Popup disabled. Change 'AuthorizeService.js:AuthorizeService._popupDisabled' to false to enable it.",
                )
            }

            await this.userManager.signoutPopup(this.createArguments())
            this.updateState(undefined)
            return this.success(state)
        } catch (popupSignOutError) {
            console.log('Popup signout error: ', popupSignOutError)
            try {
                await this.userManager.signoutRedirect(this.createArguments(state))
                return this.redirect()
            } catch (redirectSignOutError) {
                console.log('Redirect signout error: ', redirectSignOutError)
                return this.error(redirectSignOutError)
            }
        }
    }

    async completeSignOut(url) {
        // await this.ensureUserManagerInitialized()
        try {
            const response = null //await this.userManager.signoutCallback(url)
            this.updateState(null)
            return this.success(response && response.data)
        } catch (error) {
            console.log(`There was an error trying to log out '${error}'.`)
            return this.error(error)
        }
    }

    updateState() {
        this._isAuthenticated = !!this._token
        this.notifySubscribers()
    }

    subscribe(callback) {
        this._callbacks.push({ callback, subscription: this._nextSubscriptionId++ })
        return this._nextSubscriptionId - 1
    }

    unsubscribe(subscriptionId) {
        const subscriptionIndex = this._callbacks
            .map((element, index) =>
                element.subscription === subscriptionId ? { found: true, index } : { found: false },
            )
            .filter((element) => element.found === true)
        if (subscriptionIndex.length !== 1) {
            throw new Error(`Found an invalid number of subscriptions ${subscriptionIndex.length}`)
        }

        this._callbacks.splice(subscriptionIndex[0].index, 1)
    }

    notifySubscribers() {
        for (let i = 0; i < this._callbacks.length; i++) {
            const callback = this._callbacks[i].callback
            callback()
        }
    }

    createArguments(state) {
        return { useReplaceToNavigate: true, data: state }
    }

    error(message) {
        return { status: AuthenticationResultStatus.Fail, message }
    }

    success() {
        return { status: AuthenticationResultStatus.Success }
    }

    redirect() {
        return { status: AuthenticationResultStatus.Redirect }
    }
}

const authService = new AuthorizeService()

export default authService
