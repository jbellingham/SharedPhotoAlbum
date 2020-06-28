import React, { useEffect } from 'react'
import { observer } from 'mobx-react'
import { Row, Col } from 'react-bootstrap'
import Post from '../Post'
import NewPost from './NewPost'
import { useStore } from '../../stores/StoreContext'
import NewFeed from './NewFeed'
import { useParams } from 'react-router-dom'

const Feed = observer(() => {
    const { feedStore } = useStore()
    const { feedId } = useParams()

    useEffect(() => {
        feedStore.getFeed(feedId)
    }, [])

    const { posts, feed } = feedStore

    return (
        <div className="feed-container">
            <NewFeed />
            <Row>
                <Col md={{ span: 4, offset: 4 }}>
                    <div className="feed-name">{feed?.name}</div>
                </Col>
                <Col md={{ span: 4, offset: 4 }}>
                    <NewPost />
                </Col>
            </Row>
            {posts &&
                posts.map((post) => {
                    return (
                        <Row key={post.id}>
                            <Col md={{ span: 4, offset: 4 }}>
                                <Post {...post} />
                            </Col>
                        </Row>
                    )
                })}
        </div>
    )
})

export default Feed
