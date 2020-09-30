import { CreateFeedCommand, FeedVm, IFeedDto, IFeedsClient, IPostDto } from '../Client'
import { action, computed, observable } from 'mobx'
import PostStore from './PostStore'

class FeedStore {
    constructor(private postStore: PostStore, private feedsClient: IFeedsClient) {
        this.getFeeds()
    }

    @observable
    feedName: string | undefined = ""

    @observable
    isLoading: boolean = false

    @observable
    feeds: IFeedDto[] = []

    @computed
    get myFeeds(): IFeedDto[] {
        return this.feeds.filter(_ => _.isOwner)
    }

    @computed
    get subscriptions(): IFeedDto[] {
        return this.feeds.filter(_ => _.isSubscription)
    }

    @action
    async getFeed(feedId: string | null): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(feedId).then((result) => {
                this.feedName = result.name
                this.isLoading = false
                if (result.posts) {
                    this.postStore.posts?.push(...result.posts)
                    this.isLoading = false
                }
            })
        }
    }

    @action
    async getFeeds(): Promise<void> {
        if (!this.isLoading) {
            this.isLoading = true
            this.feedsClient.get(null).then(({ feeds }) => {
                if (feeds?.length > 0) {
                    this.feeds.push(...feeds)
                    this.isLoading = false
                }
            })
        }
    }

    async createFeed(feed: CreateFeedCommand): Promise<string> {
        return await this.feedsClient.create(CreateFeedCommand.fromJS({ ...feed }))
    }


}

export default FeedStore