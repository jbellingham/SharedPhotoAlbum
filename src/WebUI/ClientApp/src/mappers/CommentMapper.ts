import { CommentDto } from '../Client'
import { Comment } from '../components/models/Comment'

export class CommentMapper {
    static fromDto(dto: CommentDto): Comment | undefined {
        if (dto.id !== undefined && dto.text !== undefined && dto.likes !== undefined && dto.postId !== undefined) {
            return new Comment(dto.id, dto.text, dto.likes, dto.postId)
        }
    }
}
