import { action, observable } from "mobx"
import { IUserClient } from "../Client"

class UserStore {
    @observable
    profilePictureUrl: string = ""

    @observable
    loading: boolean = false

    constructor(userClient: IUserClient) {
        this.loading = true
        userClient.profilePictureUrl().then((result) => {
            this.profilePictureUrl = result
            this.loading = false
        })
    }
}

export default UserStore