import gql from 'graphql-tag'
import { debounce } from 'lodash'
import { observer } from 'mobx-react'
import React, { useEffect } from 'react'
import { useQuery } from 'react-apollo'
import Post from '..'
import { useStore } from '../../../stores/StoreContext'

// export const GET_POSTS = gql`
//     query postsByFeedId($feedId: String!, $skip: Int, $limit: Int) {
//         postsByFeedId(feedId: $feedId, skip: $skip, limit: $limit) {
//             _id
//             text
//             poster {
//                 _id
//                 email
//             }
//             media {
//                 _id
//                 publicId
//                 mimeType
//             }
//         }
//     }
// `

interface PostListProps {
    feedId: string
}

const PostList = observer((props: PostListProps) => {
    const offset = 200
    // const { data, loading, fetchMore } = useQuery(GET_POSTS, {
    //     variables: {
    //         feedId: props.feedId,
    //         skip: 0,
    //         limit: 5,
    //     },
    //     fetchPolicy: 'cache-and-network',
    // })
    const { postStore } = useStore()
    const { posts } = postStore
    useEffect(() => {
        postStore.getPosts(props.feedId)
    }, [])

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
    return posts?.length > 0 ? <>{posts?.map((post) => <Post post={post} key={post.id} />)}</> : <span>Loading ...</span>
})

export default PostList
