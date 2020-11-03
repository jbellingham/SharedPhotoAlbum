import { action, observable } from 'mobx'
import { AuthClient, AuthenticationResponse } from '../Client'
import Cookies from 'js-cookie'

class AuthStore {
    @observable
    isAuthenticated: boolean | undefined = false

    @observable
    token: string | undefined = undefined

    @observable
    tokenExpiry: Date | undefined = undefined

    @observable
    refreshToken: string | undefined = undefined

    constructor(private tokenClient: AuthClient) {
        this.token = Cookies.get('auth_token')
        const dateString = Cookies.get('auth_token_expires')
        this.tokenExpiry = new Date(dateString)
    }

    @action
    async authenticate(): Promise<void> {
        if (this.token && this.tokenExpiry && new Date() < this.tokenExpiry) {
            this.isAuthenticated = true
        } else if (this.token && this.refreshToken && this.tokenExpiry && new Date() >= this.tokenExpiry) {
            return this.tokenClient
                .refreshToken(this.token, this.refreshToken)
                .then((response) => this.handleAuthenticationResponse(response))
        } else {
            return this.tokenClient
                .authenticate()
                .then((response) => this.handleAuthenticationResponse(response))
                .catch((error) => {
                    console.log(error)
                })
        }
    }

    private handleAuthenticationResponse(response: AuthenticationResponse): void {
        this.isAuthenticated = response.isAuthenticated
        this.token = response.authToken?.tokenString
        this.tokenExpiry = response.authToken?.validTo
        this.refreshToken = response.refreshToken?.tokenString

        this.token && Cookies.set('auth_token', response.authToken?.tokenString)
        this.tokenExpiry && Cookies.set('auth_token_expires', response.authToken?.validTo)
        this.isAuthenticated = true
        console.log(response)
    }
}

export default AuthStore
