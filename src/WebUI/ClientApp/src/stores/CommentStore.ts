import { action, decorate } from 'mobx'
import { CommentClient, CreateCommentCommand, PostDto, CommentDto } from '../Client'
import PostStore from './PostStore'

class CommentStore {
    constructor(private commentClient: CommentClient, private postStore: PostStore) {}

    async createComment(comment: CreateCommentCommand): Promise<void> {
        const id = await this.commentClient.create(comment)
        const post = this.postStore.posts?.find((post: PostDto) => post.id === comment.postId)
        if (post && !post?.comments) {
            post.comments = []
        }
        post?.comments?.push({
            ...comment,
            id,
            init: function () {
                return
            },
            toJSON: function () {
                return
            },
        })
    }
}

decorate(CommentStore, {
    createComment: action,
})

export default CommentStore
