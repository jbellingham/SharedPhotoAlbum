import { CreateFeedCommand, IFeedsClient, IFeedVm, PostDto } from '../Client'
import { decorate, action, observable } from 'mobx'

class FeedStore {
    posts: PostDto[] | undefined = []
    isLoading = false

    constructor(private feedsClient: IFeedsClient) {
        this.getFeed(null)
    }

    async getFeed(feedId: number | null): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(feedId).then((result) => {
                this.isLoading = false
                this.posts = result.posts
            })
        }
    }

    async createFeed(feed: CreateFeedCommand): Promise<number> {
        return await this.feedsClient.create(CreateFeedCommand.fromJS({ ...feed }))
    }
}

decorate(FeedStore, {
    posts: observable,
    getFeed: action,
    createFeed: action,
})

export default FeedStore
