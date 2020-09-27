import React from 'react'
import NewFeed from '../NewFeed'
import { Button } from 'react-bootstrap'
import gql from 'graphql-tag'
import { useQuery } from 'react-apollo'

interface IFeedButtonProps {
    feedId: string | undefined
    feedName: string
}

export interface IFeedListProps {
    onFeedSelected: (feedId: string) => void
    selectedFeed: string
}

const GET_FEEDS = gql`
    query feeds {
        feeds {
            _id
            name
            isOwner
            isActiveSubscription
            isPendingSubscription
        }
    }
`

function FeedList(props: IFeedListProps): JSX.Element {
    const [selectedFeedId, setSelectedFeedId] = React.useState(props.selectedFeed)
    const [showNewFeedModal, setShowNewFeedModal] = React.useState(false)

    const handleNewFeedModalClose = () => {
        setShowNewFeedModal(!showNewFeedModal)
    }

    const { data, loading } = useQuery(GET_FEEDS)

    const feedsList =
        !loading &&
        data.feeds.filter((_) => _.isOwner).map((_: { _id: any; name: any }) => ({ feedId: _._id, feedName: _.name }))

    const subscriptionsList =
        !loading &&
        data.feeds
            .filter((_) => _.isActiveSubscription)
            .map((_: { _id: any; name: any }) => ({ feedId: _._id, feedName: _.name }))

    if (props.selectedFeed && selectedFeedId !== props.selectedFeed) {
        setSelectedFeedId(props.selectedFeed)
    }

    const onButtonClick = (feedId: string) => {
        setSelectedFeedId(feedId)
        props.onFeedSelected(feedId)
    }

    const renderButton = (feed: IFeedButtonProps) => {
        return (
            <p key={feed.feedId}>
                <Button
                    onClick={() => onButtonClick(feed.feedId || '')}
                    variant={selectedFeedId === feed.feedId ? 'primary' : 'outline-primary'}
                >
                    {feed.feedName}
                </Button>
            </p>
        )
    }

    return (
        <div className="feed-list-container">
            <div className="mb-2">
                <Button variant="primary" onClick={() => setShowNewFeedModal(!showNewFeedModal)}>
                    Create new feed
                </Button>
            </div>
            <NewFeed show={showNewFeedModal} handleClose={handleNewFeedModalClose} />
            <div className="mb-4">
                <h5>My Feeds</h5>
                {feedsList.length > 0 ? (
                    feedsList.map((feed: IFeedButtonProps) => renderButton(feed))
                ) : (
                    <span>No feeds to display.</span>
                )}
            </div>
            <h5>Subscriptions</h5>
            {subscriptionsList.length > 0 ? (
                subscriptionsList.map((subscription: IFeedButtonProps) => renderButton(subscription))
            ) : (
                <span>No subscriptions to display.</span>
            )}
        </div>
    )
}

export default FeedList
