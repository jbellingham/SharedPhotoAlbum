import { CreateFeedCommand, FeedDto, IFeedsClient } from '../Client'
import { action, computed, observable } from 'mobx'
import PostStore from './PostStore'

class FeedStore {
    constructor(private postStore: PostStore, private feedsClient: IFeedsClient) {
        this.getFeeds()
    }

    currentFeedId: string | undefined = ''

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

    @computed
    get canViewCurrentFeed(): boolean {
        if (!this.currentFeedId) return false
        return this.myFeeds.some((feed) => feed.id === this.currentFeedId) ||
        this.subscriptions.some((feed) => feed.id === this.currentFeedId)
    }

    @action
    async getFeed(feedId: string | null): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(feedId).then((result) => {
                if (result !== undefined && result.feeds?.length === 1) {
                    this.feedName = result.feeds[0].name
                }
                this.isLoading = false
            })
        }
    }

    @action
    setCurrentFeed(feedId: string): void {
        this.currentFeedId = feedId
    }

    @action
    async getFeeds(): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(null).then(({ feeds }) => {
                if (feeds !== undefined &&
                    feeds.length > 0) {
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
