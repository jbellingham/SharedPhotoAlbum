import gql from 'graphql-tag'
import { debounce } from 'lodash'
import React from 'react'
import { useQuery } from 'react-apollo'
import Post from '.'

export const GET_POSTS = gql`
    query postsByFeedId($feedId: String!, $skip: Int, $limit: Int) {
        postsByFeedId(feedId: $feedId, skip: $skip, limit: $limit) {
            _id
            text
            poster {
                _id
                email
            }
            media {
                _id
                publicId
                mimeType
            }
        }
    }
`

interface PostListProps {
    feedId: string
}

function PostList(props: PostListProps): JSX.Element {
    const offset = 200
    const { data, loading, fetchMore } = useQuery(GET_POSTS, {
        variables: {
            feedId: props.feedId,
            skip: 0,
            limit: 5,
        },
        fetchPolicy: 'cache-and-network',
    })

    const { postsByFeedId: posts } = data || {}

    window.onscroll = debounce(() => {
        if (loading) return

        if (window.innerHeight + document.documentElement.scrollTop + offset >= document.documentElement.offsetHeight) {
            fetchMore({
                variables: {
                    feedId: props.feedId,
                    skip: posts.length,
                    limit: 5,
                },
                updateQuery: (prev, { fetchMoreResult }) => {
                    if (!fetchMoreResult) return prev
                    return Object.assign({}, prev, {
                        postsByFeedId: [...prev.postsByFeedId, ...fetchMoreResult.postsByFeedId],
                    })
                },
            })
        }
    }, 100)
    return posts?.length > 0 ? posts?.map((post) => <Post post={post} key={post._id} />) : <span>Loading ...</span>
}

export default PostList
