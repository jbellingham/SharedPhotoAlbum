import { action, observable } from 'mobx'
import { IPostsClient, CreatePostCommand, IPostDto } from '../Client'

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
}

export default PostStore