import { PostDto } from '../Client'
import { Post } from '../components/models/Post'
import { Comment } from '../components/models/Comment'
import { CommentMapper } from './CommentMapper'
import { MediaMapper } from './MediaMapper'

export class PostMapper {
    static fromDto(dto: PostDto): Post {
        const comments = dto.comments?.map((comment) => CommentMapper.fromDto(comment)) as Comment[]
        const media = dto.storedMedia?.map((media) => MediaMapper.fromDto(media))
        return new Post(dto.id, dto.text, dto.linkUrl, comments, media)
    }
}
