import Cookies from 'js-cookie'

export const AuthenticationResultStatus = {
    Redirect: 'redirect',
    Success: 'success',
    Fail: 'fail',
}

export class AuthorizeService {
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

    async onStatusChange(response) {
        if (response.status === 'connected') {
            await this.signIn()
        }
    }

    // We try to authenticate the user in three different ways:
    // 1) We try to see if we can authenticate the user silently. This happens
    //    when the user is already logged in on the IdP and is done using a hidden iframe
    //    on the client.
    // 2) We try to authenticate the user using a PopUp Window. This might fail if there is a
    //    Pop-Up blocker or the user has disabled PopUps.
    // 3) If the two methods above fail, we redirect the browser to the IdP to perform a traditional
    //    redirect flow.
    async signIn(state) {
        try {
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
                this._isAuthenticated = true
                return this.success()
            } else {
                throw new Error('poop')
            }
        } catch (silentError) {
            // User might not be authenticated, fallback to popup authentication
            console.log('Silent authentication error: ', silentError)

            try {
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

    error(message) {
        return { status: AuthenticationResultStatus.Fail, message }
    }

    success() {
        return { status: AuthenticationResultStatus.Success }
    }
}

const authService = new AuthorizeService()

export default authService
