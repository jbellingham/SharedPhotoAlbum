import { observable } from 'mobx'
import { PostDto } from '../../Client'
import { PostMapper } from '../../mappers/PostMapper'
import { Post } from './Post'

export class Feed {
    id?: string
    name?: string | undefined
    isOwner?: boolean
    isSubscription?: boolean

    @observable
    posts: Post[] = []

    constructor(
        id: string | undefined,
        name: string | undefined,
        isOwner: boolean | undefined,
        isSubscription: boolean | undefined,
        posts: Post[] | [],
    ) {
        this.id = id
        this.name = name
        this.isOwner = isOwner
        this.isSubscription = isSubscription
        this.posts = posts
    }

    updatePosts(incomingPosts: PostDto[] | undefined): void {
        if (incomingPosts !== undefined && incomingPosts.length > 0) {
            const posts = incomingPosts.map((comment) => PostMapper.fromDto(comment)) as Post[]
            this.posts = posts
        }
    }
}
