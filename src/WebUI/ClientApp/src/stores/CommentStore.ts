import { action } from 'mobx'
import { CommentsClient, CreateCommentCommand } from '../Client'
import PostStore from './PostStore'

class CommentStore {
    constructor(private commentsClient: CommentsClient, private postStore: PostStore) {}

    @action
    async createComment(comment: CreateCommentCommand): Promise<void> {
        await this.commentsClient.create(comment)
        await this.postStore.updatePost(comment.postId)
    }
}

export default CommentStore
