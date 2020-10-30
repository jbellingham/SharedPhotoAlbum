import { UserDetailsDto } from '../Client'
import { User } from '../components/models/User'

export class UserMapper {
    static fromDto(dto: UserDetailsDto): User {
        return new User(dto.firstName, dto.lastName, dto.profilePictureUrl)
    }
}
