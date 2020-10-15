import { action, observable } from 'mobx'
import { TokenClient } from '../Client'
import Cookies from 'js-cookie'

class AuthStore {
    @observable
    isAuthenticated = false

    @observable
    token = ''

    constructor(private tokenClient: TokenClient) {}

    @action
    async getToken(): Promise<void> {
        const token = Cookies.get('auth_token')
        if (token) {
            this.token = token
        } else {
            return this.tokenClient
                .get()
                .then((response) => {
                    // this.token = await tokenResponse.data
                    // Cookies.set('auth_token')
                    this.isAuthenticated = true
                    console.log(response)
                })
                .catch((error) => {
                    console.log(error)
                })
        }
    }
}

export default AuthStore
