import { FeedDto } from '../Client'
import { Feed } from '../components/models/Feed'

export class FeedMapper {
    static fromDto(dto: FeedDto): Feed {
        return new Feed(dto.id, dto.name, dto.isOwner, dto.isSubscription, [])
    }
}
