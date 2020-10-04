import { action, observable } from 'mobx'
import { IPostsClient, CreatePostCommand, ICommentsClient } from '../Client'
import { PostMapper } from '../mappers/PostMapper'
import { Post } from '../components/models/Post'

class PostStore {
    @observable
    posts: Post[] = []

    constructor(private postClient: IPostsClient, private commentsClient: ICommentsClient) {}

    @action
    async createPost(post: CreatePostCommand): Promise<void> {
        await this.postClient.create(post)
        await this.getPosts(post.feedId)
    }

    @action
    async getPosts(feedId: string): Promise<void> {
        const { posts } = await this.postClient.get(feedId)
        this.posts = posts?.map((dto) => PostMapper.fromDto(dto))
    }

    @action
    async updatePost(postId: string | undefined): Promise<void> {
        const { comments } = await this.commentsClient.get(postId)
        const post = this.posts.find((_) => _.id === postId)
        post?.updateComments(comments)
    }

    getPostById(postId: string | undefined): Post | undefined {
        return this.posts.find((_) => _.id === postId)
    }
}

export default PostStore
