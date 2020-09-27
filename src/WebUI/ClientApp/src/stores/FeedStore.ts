import { CreateFeedCommand, IFeedsClient, IPostDto, IFeedVm } from '../Client'
import { decorate, action, observable } from 'mobx'

class FeedStore {
    posts: IPostDto[] | undefined = []
    isLoading = false
    feed: IFeedVm | undefined

    constructor(private feedsClient: IFeedsClient) {}

    async getFeed(feedId: number | null): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(feedId).then((result) => {
                this.feed = result
                this.isLoading = false
                if (result.posts) {
                    this.posts?.push(...result.posts)
                }
            })
        }
    }

    async createFeed(feed: CreateFeedCommand): Promise<number> {
        return await this.feedsClient.create(CreateFeedCommand.fromJS({ ...feed }))
    }
}

decorate(FeedStore, {
    feed: observable,
    posts: observable,
    getFeed: action,
    createFeed: action,
})

export default FeedStore