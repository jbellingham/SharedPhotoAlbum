import { action, decorate } from 'mobx'
import { CommentsClient, CreateCommentCommand, ICommentDto } from '../Client'
import PostStore from './PostStore'

class CommentStore {
    constructor(private commentClient: CommentsClient, private postStore: PostStore) {}

    @action
    async createComment(comment: CreateCommentCommand): Promise<void> {
        const id = await this.commentClient.create(comment)
        this.postStore.updateComments(comment, id)
    }

    @action
    getComments(postId: string | undefined): ICommentDto[] {
        const post = this.postStore.getPostById(postId)
        return post?.comments || []
    }
}

export default CommentStore