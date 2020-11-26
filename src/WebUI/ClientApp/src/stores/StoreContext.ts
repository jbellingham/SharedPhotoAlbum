import { createContext, useContext } from 'react'
import PostStore from './PostStore'
import FeedStore from './FeedStore'
import CommentStore from './CommentStore'
import UserStore from './UserStore'
import { PostsClient, CommentsClient, FeedsClient, UserClient, AuthClient } from '../Client'
import Axios from 'axios'
import authService from '../components/api-authorization/AuthorizeService'

const baseUrl = ''

const axios = Axios.create()
axios.interceptors.request.use(async function (config) {
    const token = await authService.getAccessToken()

    if (token) {
        config.headers = {
            ...config.headers,
            Authorization: `Bearer ${token}`,
        }
    }
    return config
})

const clients = {
    commentsClient: new CommentsClient(baseUrl, axios),
    feedsClient: new FeedsClient(baseUrl, axios),
    userClient: new UserClient(baseUrl, axios),
}

const postStore = new PostStore(new PostsClient(baseUrl, axios), clients.commentsClient)
const commentStore = new CommentStore(clients.commentsClient, postStore)
const feedStore = new FeedStore(clients.feedsClient)
const userStore = new UserStore(clients.userClient)

export interface IStore {
    postStore: PostStore
    commentStore: CommentStore
    feedStore: FeedStore
    userStore: UserStore
}

export const store: IStore = {
    postStore,
    commentStore,
    feedStore,
    userStore,
}

export const StoreContext = createContext(store)

export const useStore = (): IStore => {
    return useContext(StoreContext)
}
