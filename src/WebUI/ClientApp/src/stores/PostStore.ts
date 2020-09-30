import { action, observable } from 'mobx'
import { IPostsClient, CreatePostCommand, IPostDto, CreateCommentCommand } from '../Client'

class PostStore {
    @observable
    posts: IPostDto[] = []

    constructor(private postClient: IPostsClient) { }

    @action
    async createPost(post: CreatePostCommand): Promise<void> {
        await this.postClient.create(CreatePostCommand.fromJS({ ...post }))
        this.posts?.unshift(post)
    }

    @action
    async getPosts(feedId: string): Promise<void> {
        const { posts } = await this.postClient.get(feedId)
        this.posts.push(...posts)
    }

    @action
    updateComments(comment: CreateCommentCommand, commentId: string) {
        //this.getPostById(comment.postId)
        this.posts.find(_ => _.id === comment.postId)?.comments?.push({
            ...comment,
            id: commentId,
            init: function () {
                return
            },
            toJSON: function () {
                return
            },
        })
        // if (post && !post?.comments) {
        //     post.comments = []
        // }
        // post?.comments?.push({
        //     ...comment,
        //     id: commentId,
        //     init: function () {
        //         return
        //     },
        //     toJSON: function () {
        //         return
        //     },
        // })
    }

    getPostById(postId: string | undefined): IPostDto | undefined {
        return this.posts.find(_ => _.id === postId)
    }
}

export default PostStore