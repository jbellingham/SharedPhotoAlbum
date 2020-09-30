import React from 'react'

import { observer } from "mobx-react";
import { IFeedDto } from "../../../Client";
import { Button } from 'react-bootstrap';
import { useHistory, useParams } from 'react-router-dom';

export interface ICategorisedFeedsProps {
    feeds: IFeedDto[]
    heading: string
    emptyListMessage: string
}

const CategorisedFeeds = observer((props: ICategorisedFeedsProps) => {
    const { feedId } = useParams()
    const [selectedFeedId, setselectedFeedId] = React.useState(feedId)
    const history = useHistory()

    const onButtonClick = (feedId: string) => {
        setselectedFeedId(feedId)
        history.push(feedId)
    }

    const renderButton = (feed: IFeedDto) => {
        const { id, name } = feed
        return (
            <p key={id}>
                <Button
                    onClick={() => onButtonClick(id || '')}
                    variant={selectedFeedId === id ? 'primary' : 'outline-primary'}
                >
                    {name}
                </Button>
            </p>
        )
    }
    
    const { feeds, heading, emptyListMessage } = props

    return <>
        <h5>{heading}</h5>
        {feeds.length > 0 ? (
            feeds.map(feed => renderButton(feed))
        ) : (
            <span>{emptyListMessage}</span>
        )}
    </>
})

export default CategorisedFeeds