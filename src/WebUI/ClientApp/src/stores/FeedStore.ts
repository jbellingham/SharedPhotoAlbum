import { CreateFeedCommand, FeedDto, IFeedsClient } from '../Client'
import { action, computed, observable } from 'mobx'
import PostStore from './PostStore'

class FeedStore {
    constructor(private postStore: PostStore, private feedsClient: IFeedsClient) {}

    @observable
    feedName: string | undefined = ''

    @observable
    isLoading = false

    @observable
    feeds: FeedDto[] = []

    @computed
    get myFeeds(): FeedDto[] {
        return this.feeds.filter((_) => _.isOwner)
    }

    @computed
    get subscriptions(): FeedDto[] {
        return this.feeds.filter((_) => _.isSubscription)
    }

    @action
    async getFeed(feedId: string | null): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(feedId).then((result) => {
                this.feedName = result.name
                this.isLoading = false
            })
        }
    }

    @action
    async getFeeds(): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(null).then(({ feeds }) => {
                if (feeds?.length > 0) {
                    this.feeds = feeds
                    this.isLoading = false
                }
            })
        }
    }

    async createFeed(feed: CreateFeedCommand): Promise<string> {
        return await this.feedsClient.create(feed)
    }
}

export default FeedStore
