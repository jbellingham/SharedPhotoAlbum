import { action, decorate, observable } from 'mobx'
import { PostDto, IPostsClient, CreatePostCommand } from '../Client'

class PostStore {
    @observable
    posts: PostDto[] = []

    constructor(private postClient: IPostsClient) {}

    async createPost(post: CreatePostCommand): Promise<void> {
        await this.postClient.create(CreatePostCommand.fromJS({ ...post }))
        this.posts?.unshift(post)
    }
}

export default PostStore