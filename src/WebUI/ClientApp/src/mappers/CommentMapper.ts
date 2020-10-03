import { CommentDto } from '../Client'
import { Comment } from '../components/models/Comment'

export class CommentMapper {
    static fromDto(dto: CommentDto): Comment {
        return new Comment(dto.id, dto.text, dto.likes, dto.postId)
    }
}
