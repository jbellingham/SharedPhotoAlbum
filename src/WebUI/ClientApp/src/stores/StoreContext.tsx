import { createContext, useContext } from 'react'
import PostStore from './PostStore'
import FeedStore from './FeedStore'
import CommentStore from './CommentStore'
import { PostsClient, CommentsClient, FeedsClient } from '../Client'
import Axios from 'axios'
import authService from '../components/api-authorization/AuthorizeService'

const baseUrl = 'https://localhost:44320'
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
const feedStore = new FeedStore(new FeedsClient(baseUrl, axios))

export interface IStore {
    postStore: PostStore
    commentStore: CommentStore
    feedStore: FeedStore
}

export const store: IStore = {
    postStore,
    commentStore,
    feedStore,
}

export const StoreContext = createContext(store)

export const useStore = (): IStore => {
    return useContext(StoreContext)
}
