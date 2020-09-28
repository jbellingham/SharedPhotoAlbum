import { action, decorate, observable } from 'mobx'
import { PostDto, IPostsClient, CreatePostCommand } from '../Client'

class PostStore {
    posts: PostDto[] = []

    constructor(private postClient: IPostsClient) {}

    async createPost(post: CreatePostCommand): Promise<void> {
        await this.postClient.create(CreatePostCommand.fromJS({ ...post }))
        this.posts?.unshift(post)
    }
}

decorate(PostStore, {
    posts: observable,
    createPost: action,
})

export default PostStore