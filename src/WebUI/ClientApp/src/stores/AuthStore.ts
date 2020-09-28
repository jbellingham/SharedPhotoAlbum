import { decorate, observable } from 'mobx'
import authService, { AuthorizeService } from '../components/api-authorization/AuthorizeService'

class AuthStore {
    isAuthenticated = false

    constructor(authService: AuthorizeService) {
        authService.isAuthenticated().then((authenticated) => {
            this.isAuthenticated = authenticated
        })

        // authService.getAccessToken().then((token) => {
        //     this.token = token
        // })
    }

    // public async ensureToken() {
    //     if (authService.tokenExpired()) {
    //         await authService.getAccessToken()
    //     }
    // }
}

decorate(AuthStore, {
    isAuthenticated: observable,
    // token: observable,
})

export default AuthStore