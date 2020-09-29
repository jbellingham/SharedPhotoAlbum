import { CreateFeedCommand, IFeedsClient, IPostDto } from '../Client'
import { decorate, action, observable } from 'mobx'
import PostStore from './PostStore'

class FeedStore {
    @observable
    feedName: string | undefined = ""

    @observable
    isLoading: boolean = false

    constructor(private postStore: PostStore, private feedsClient: IFeedsClient) { }

    async getFeed(feedId: string | null): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(feedId).then((result) => {
                this.feedName = result.name
                this.isLoading = false
                if (result.posts) {
                    this.postStore.posts?.push(...result.posts)
                    //this.posts?.push(...result.posts)
                }
            })
        }
    }

    async createFeed(feed: CreateFeedCommand): Promise<string> {
        return await this.feedsClient.create(CreateFeedCommand.fromJS({ ...feed }))
    }
}

export default FeedStore