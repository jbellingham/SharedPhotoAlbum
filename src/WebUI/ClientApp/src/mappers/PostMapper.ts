import { PostDto } from '../Client'
import { Post } from '../components/models/Post'
import { CommentMapper } from './CommentMapper'

export class PostMapper {
    static fromDto(dto: PostDto): Post {
        const comments = dto.comments?.map((comment) => CommentMapper.fromDto(comment))
        return new Post(dto.id, dto.text, dto.linkUrl, comments, dto.storedMedia)
    }
}
