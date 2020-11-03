import { createContext, useContext } from 'react'
import PostStore from './PostStore'
import FeedStore from './FeedStore'
import CommentStore from './CommentStore'
import AuthStore from './AuthStore'
import UserStore from './UserStore'
import { PostsClient, CommentsClient, FeedsClient, UserClient, AuthClient } from '../Client'
import Axios from 'axios'

const baseUrl = ''
const authStore = new AuthStore(new AuthClient(baseUrl, Axios.create()))

const axios = Axios.create()
axios.interceptors.request.use(async function (config) {
    if (!authStore.isAuthenticated) {
        await authStore.authenticate()
    }
    const token = authStore.token

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
const feedStore = new FeedStore(postStore, clients.feedsClient)
const userStore = new UserStore(clients.userClient)

export interface IStore {
    postStore: PostStore
    commentStore: CommentStore
    feedStore: FeedStore
    authStore: AuthStore
    userStore: UserStore
}

export const store: IStore = {
    postStore,
    commentStore,
    feedStore,
    authStore,
    userStore,
}

export const StoreContext = createContext(store)

export const useStore = (): IStore => {
    return useContext(StoreContext)
}
