import { createContext, useContext } from 'react'
import PostStore from './PostStore'
import FeedStore from './FeedStore'
import CommentStore from './CommentStore'
import { PostsClient, CommentsClient, FeedsClient } from '../Client'
import Axios from 'axios'
import { AuthorizeService } from '../components/api-authorization/AuthorizeService'
import AuthStore from './AuthStore'

const authService = new AuthorizeService()
const baseUrl = 'https://localhost:5001'
const authStore = new AuthStore(authService)

const axios = Axios.create()
axios.interceptors.request.use(async function (config) {
    const token = await authService.getAccessToken()

    config.headers = {
        ...config.headers,
        Authorization: `Bearer ${token}`,
    }
    return config
})

const postStore = new PostStore(new PostsClient(baseUrl, axios))
const commentStore = new CommentStore(new CommentsClient(baseUrl, axios), postStore)
const feedStore = new FeedStore(postStore, new FeedsClient(baseUrl, axios))

export interface IStore {
    postStore: PostStore
    commentStore: CommentStore
    feedStore: FeedStore
    authStore: AuthStore
}

export const store: IStore = {
    postStore,
    commentStore,
    feedStore,
    authStore,
}

export const StoreContext = createContext(store)

export const useStore = (): IStore => {
    return useContext(StoreContext)
}