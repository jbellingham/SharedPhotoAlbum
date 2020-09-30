import React from 'react'
import NewFeed from '../NewFeed'
import { Button } from 'react-bootstrap'
import { useStore } from '../../../stores/StoreContext'
import { observer } from 'mobx-react'
import CategorisedFeeds, { ICategorisedFeedsProps } from './CategorisedFeeds'


// export interface IFeedListProps {
//     onFeedSelected: (feedId: string) => void
//     selectedFeed: string
// }

const FeedList = observer(() => {
    // const [selectedFeedId, setSelectedFeedId] = React.useState(props.selectedFeed)
    const [showNewFeedModal, setShowNewFeedModal] = React.useState(false)

    const { feedStore } = useStore()

    const { myFeeds, subscriptions } = feedStore

    const handleNewFeedModalClose = () => {
        setShowNewFeedModal(!showNewFeedModal)
    }

    // if (props.selectedFeed && selectedFeedId !== props.selectedFeed) {
    //     setSelectedFeedId(props.selectedFeed)
    // }

    const myFeedsProps: ICategorisedFeedsProps = {
        feeds: myFeeds,
        heading: "My Feeds",
        emptyListMessage: "No feeds to display."
    }

    const subscriptionsProps: ICategorisedFeedsProps = {
        feeds: subscriptions,
        heading: "My Subscriptions",
        emptyListMessage: "No subscriptions to display."
    }

    return (
        feedStore.isLoading ? <span>Loading</span> :
        <div className="feed-list-container">
            <div className="mb-2">
                <Button variant="primary" onClick={() => setShowNewFeedModal(!showNewFeedModal)}>
                    Create new feed
                </Button>
            </div>
            <NewFeed show={showNewFeedModal} handleClose={handleNewFeedModalClose} />
            <div className="mb-4">
                <CategorisedFeeds {...myFeedsProps} />
            </div>
            <CategorisedFeeds {...subscriptionsProps} />
        </div>
    )
})

export default FeedList
