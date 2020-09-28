import { CreateFeedCommand, IFeedsClient, IPostDto, IFeedVm } from '../Client'
import { decorate, action, observable } from 'mobx'

class FeedStore {
    posts: IPostDto[] = []
    isLoading = false
    feed: IFeedVm = observable({
        name: ""
    })

    constructor(private feedsClient: IFeedsClient) {}

    async getFeed(feedId: string | null): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(feedId).then((result) => {
                this.feed.name = result.name
                this.isLoading = false
                if (result.posts) {
                    this.posts?.push(...result.posts)
                }
            })
        }
    }

    async createFeed(feed: CreateFeedCommand): Promise<string> {
        return await this.feedsClient.create(CreateFeedCommand.fromJS({ ...feed }))
    }
}

decorate(FeedStore, {
    isLoading: observable,
    feed: observable,
    posts: observable,
    getFeed: action,
    createFeed: action,
})

export default FeedStore