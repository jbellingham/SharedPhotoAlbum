import React from 'react'
import NewPost from './NewPost'
import { useParams } from 'react-router-dom'
import { Col, Row, Container } from 'react-bootstrap'
import SubscriptionRequests from './SubscriptionRequests'
import gql from 'graphql-tag'
import { useQuery } from 'react-apollo'
import NewFeed from './NewFeed'
import PostList from '../Post/PostList'

export const GET_FEED = gql`
    query feedById($id: String) {
        feedById(_id: $id) {
            _id
            name
            ownerId
            isOwner
            isActiveSubscription
        }
    }
`

interface FeedParams {
    feedId: string
}

function Feed(): JSX.Element {
    const [showNewFeedModal, setShowNewFeedModal] = React.useState(false)
    const toggleNewFeedModal = (e: React.MouseEvent) => {
        e.preventDefault()
        setShowNewFeedModal(!showNewFeedModal)
    }

    const handleNewFeedModalClose = () => {
        setShowNewFeedModal(!showNewFeedModal)
    }

    const { feedId } = useParams<FeedParams>()

    const { data, loading, refetch } = useQuery(GET_FEED, {
        variables: {
            id: feedId,
        },
        fetchPolicy: 'cache-and-network',
    })

    const { feedById: feed } = data || {}

    const canView: boolean = (!loading && !feedId) || feed?.isOwner || feed?.isActiveSubscription

    return feed ? (
        <div className="feed-container">
            {(canView && (
                <Container fluid>
                    <Row>
                        <Col xs={{ span: 12 }} lg={{ span: 7, offset: 2 }}>
                            <h1>{feed?.name}</h1>
                            {feed.isOwner && <NewPost feedId={feedId} refetchFeed={refetch} />}
                            <PostList feedId={feed._id} />
                        </Col>
                        <Col xs={{ span: 0 }} lg={{ span: 3 }}>
                            {feed.isOwner && <SubscriptionRequests />}
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
}

export default Feed
