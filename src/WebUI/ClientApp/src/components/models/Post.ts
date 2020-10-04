import { observable } from 'mobx'
import { CommentDto } from '../../Client'
import { CommentMapper } from '../../mappers/CommentMapper'
import { Comment } from './Comment'
import { Media } from './Media'

export class Post {
    id?: string
    linkUrl?: string | undefined
    text?: string | undefined
    media?: Media[] | undefined

    @observable
    comments: Comment[] = []

    constructor(
        id: string | undefined,
        text: string | undefined,
        linkUrl: string | undefined,
        comments: Comment[] = [],
        media: Media[] = [],
    ) {
        this.id = id
        this.linkUrl = linkUrl
        this.text = text
        this.comments = comments
        this.media = media
    }

    updateComments(incomingComments: CommentDto[] | undefined): void {
        const comments = incomingComments?.map((comment) => CommentMapper.fromDto(comment)) || []
        this.comments = comments
    }
}
