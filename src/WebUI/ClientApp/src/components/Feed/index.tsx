import React, { useEffect } from 'react'
import NewPost from '../Post/NewPost'
import { useParams } from 'react-router-dom'
import { Col, Row, Container } from 'react-bootstrap'
import SubscriptionRequests from './SubscriptionRequests'
import NewFeed from './NewFeed'
import PostList from '../Post/List'
import { useStore } from '../../stores/StoreContext'
import { observer } from 'mobx-react'

interface FeedParams {
    feedId: string
}

const Feed = observer(() => {
    const [showNewFeedModal, setShowNewFeedModal] = React.useState(false)
    const { feedStore } = useStore()
    useEffect(() => {
        feedStore.getFeeds()
    }, [])
    const { feedName, isLoading } = feedStore
    const toggleNewFeedModal = (e: React.MouseEvent): void => {
        e.preventDefault()
        setShowNewFeedModal(!showNewFeedModal)
    }

    const handleNewFeedModalClose = (): void => {
        setShowNewFeedModal(!showNewFeedModal)
    }

    const { feedId } = useParams<FeedParams>()
    // feedStore.setCurrentFeed(feedId)
    useEffect(() => {
        if (feedId) {
            feedStore.getFeed(feedId)
        }
    }, [])

    const { currentFeedId } = feedStore

    const canView: boolean = !isLoading && feedStore.canViewCurrentFeed
    return canView ? (
        <div className="feed-container">
            {(!isLoading && (
                <Container fluid>
                    <Row>
                        <Col xs={{ span: 12 }} lg={{ span: 7, offset: 2 }}>
                            <h1>{feedName}</h1>
                            {/* {feed.isOwner && */}
                            {currentFeedId && (
                                <>
                                    <NewPost feedId={currentFeedId} />
                                    <PostList feedId={currentFeedId} />
                                </>
                            )}
                        </Col>
                        <Col xs={{ span: 0 }} lg={{ span: 3 }}>
                            {/* {feed.isOwner && */}
                            {/* <SubscriptionRequests /> */}
                            {/* } */}
                        </Col>
                    </Row>
                </Container>
            )) || <p>Unauthorized</p>}
        </div>
    ) : (
        <div className="vertical-center justify-content-center">
            <span className="mr-1">Please select an existing feed or</span>
            <a href="#" onClick={toggleNewFeedModal}>
                create a new one
            </a>
            .
            <NewFeed show={showNewFeedModal} handleClose={handleNewFeedModalClose} />
        </div>
    )
})

export default Feed
