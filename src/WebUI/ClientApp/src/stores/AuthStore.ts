import { decorate, observable } from 'mobx'
import { AuthorizeService } from '../components/api-authorization/AuthorizeService'

class AuthStore {
    isAuthenticated = false
    token: string | null = null

    constructor(authService: AuthorizeService) {
        authService.isAuthenticated().then((authenticated) => {
            this.isAuthenticated = authenticated
        })

        authService.getAccessToken().then((token) => {
            this.token = token
        })
    }
}

decorate(AuthStore, {
    isAuthenticated: observable,
    token: observable,
})

export default AuthStore