import React from 'react'

import { observer } from 'mobx-react'
import { IFeedDto } from '../../../Client'
import { Button } from 'react-bootstrap'
import { useHistory } from 'react-router-dom'
import { useStore } from '../../../stores/StoreContext'

export interface ICategorisedFeedsProps {
    feeds: IFeedDto[]
    heading: string
    emptyListMessage: string
}

const CategorisedFeeds = observer((props: ICategorisedFeedsProps) => {
    const history = useHistory()
    const { feedStore } = useStore()

    const onButtonClick = (feedId: string) => {
        feedStore.setCurrentFeedId(feedId)
        history.push(feedId)
    }

    const renderButton = (feed: IFeedDto) => {
        const { id, name } = feed
        return (
            <p key={id}>
                <Button
                    onClick={() => onButtonClick(id || '')}
                    variant={feedStore.currentFeedId === id ? 'primary' : 'outline-primary'}
                >
                    {name}
                </Button>
            </p>
        )
    }

    const { feeds, heading, emptyListMessage } = props

    return (
        <>
            <h5>{heading}</h5>
            {feeds.length > 0 ? feeds.map((feed) => renderButton(feed)) : <span>{emptyListMessage}</span>}
        </>
    )
})

export default CategorisedFeeds
