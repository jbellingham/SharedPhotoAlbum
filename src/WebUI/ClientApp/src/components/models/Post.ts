import { observable } from 'mobx'
import { CommentDto, StoredMediaDto } from '../../Client'
import { CommentMapper } from '../../mappers/CommentMapper'
import { Comment } from './Comment'

export class Post {
    id?: string
    linkUrl?: string | undefined
    text?: string | undefined
    storedMedia?: StoredMediaDto[] | undefined

    @observable
    comments: Comment[] = []

    constructor(id: string, text: string, linkUrl: string, comments: Comment[], storedMedia: StoredMediaDto[]) {
        this.id = id
        this.linkUrl = linkUrl
        this.text = text
        this.comments = comments
        this.storedMedia = storedMedia
    }

    updateComments(incomingComments: CommentDto[]): void {
        const comments = incomingComments.map((comment) => CommentMapper.fromDto(comment))
        this.comments = comments
    }
}
