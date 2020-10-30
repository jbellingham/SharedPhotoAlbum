import { debounce } from 'lodash'
import { observer } from 'mobx-react'
import React, { useEffect } from 'react'
import Post from '..'
import { useStore } from '../../../stores/StoreContext'

interface PostListProps {
    feedId: string
}

const PostList = observer((props: PostListProps) => {
    const offset = 200
    const { postStore } = useStore()
    const { posts } = postStore
    const { feedId } = props
    useEffect(() => {
        postStore.getPosts(feedId)
    }, [feedId])

    // const { postsByFeedId: posts } = data || {}

    // window.onscroll = debounce(() => {
    //     if (loading) return

    //     if (window.innerHeight + document.documentElement.scrollTop + offset >= document.documentElement.offsetHeight) {
    //         fetchMore({
    //             variables: {
    //                 feedId: props.feedId,
    //                 skip: posts.length,
    //                 limit: 5,
    //             },
    //             updateQuery: (prev, { fetchMoreResult }) => {
    //                 if (!fetchMoreResult) return prev
    //                 return Object.assign({}, prev, {
    //                     postsByFeedId: [...prev.postsByFeedId, ...fetchMoreResult.postsByFeedId],
    //                 })
    //             },
    //         })
    //     }
    // }, 100)
    return posts?.length > 0 ? (
        <>
            {posts?.map((post) => (
                <Post post={post} key={post.id} />
            ))}
        </>
    ) : (
        <span>There's nothing here...</span>
    )
})

export default PostList
