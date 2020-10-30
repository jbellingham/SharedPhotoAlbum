import { action, observable } from 'mobx'
import { IUserClient } from '../Client'
import { User } from '../components/models/User'
import { UserMapper } from '../mappers/UserMapper'

class UserStore {
    @observable
    loading = false

    @observable
    user: User | null = null

    constructor(private userClient: IUserClient) {}

    @action
    async getUserProfile(): Promise<void> {
        this.loading = true
        const result = await this.userClient.get()
        this.user = UserMapper.fromDto(result)
        this.loading = false
    }
}

export default UserStore
