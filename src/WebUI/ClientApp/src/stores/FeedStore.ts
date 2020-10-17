import { CreateFeedCommand, FeedDto, IFeedsClient } from '../Client'
import { action, computed, observable } from 'mobx'
import PostStore from './PostStore'
import { Feed } from '../components/models/Feed'
import { FeedMapper } from '../mappers/FeedMapper'

class FeedStore {
    constructor(private postStore: PostStore, private feedsClient: IFeedsClient) {}

    currentFeedId: string | undefined = ''

    @observable
    feedName: string | undefined = ''

    @observable
    isLoading = false

    @observable
    feeds: Feed[] = []

    @computed
    get myFeeds(): Feed[] {
        return this.feeds.filter((_) => _.isOwner)
    }

    @computed
    get subscriptions(): Feed[] {
        return this.feeds.filter((_) => _.isSubscription)
    }

    @computed
    get canViewCurrentFeed(): boolean {
        if (!this.currentFeedId) return false
        return (
            this.myFeeds.some((feed) => feed.id === this.currentFeedId) ||
            this.subscriptions.some((feed) => feed.id === this.currentFeedId)
        )
    }

    @action
    async getFeed(feedId: string | null): Promise<void> {
        if (feedId) {
            this.currentFeedId = feedId
        }
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(feedId).then(({ feeds }) => {
                if (feeds?.length === 1) {
                    this.feedName = feeds[0].name
                    this.feeds = [...this.feeds, FeedMapper.fromDto(feeds[0])]
                }
                this.isLoading = false
            })
        }
    }

    @action
    async getFeeds(): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(null).then(({ feeds }) => {
                if (feeds !== undefined && feeds.length > 0) {
                    this.feedName = feeds.find((f) => f.id === this.currentFeedId)?.name
                    this.feeds = feeds.map((feedDto: FeedDto) => FeedMapper.fromDto(feedDto))
                    this.isLoading = false
                }
            })
        }
    }

    async createFeed(feed: CreateFeedCommand): Promise<string> {
        const feedId = await this.feedsClient.create(feed)
        await this.getFeed(feedId)
        return feedId
    }
}

export default FeedStore
