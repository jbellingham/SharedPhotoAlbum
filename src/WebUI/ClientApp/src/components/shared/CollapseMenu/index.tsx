import React from 'react'
import FeedList from '../../Feed/FeedList'
import { useHistory } from 'react-router-dom'
import { EventEmitter } from 'events'

export const collapseMenuEventEmitter = new EventEmitter()

function CollapseMenu() {
    const history = useHistory()

    const onFeedSelected = (selectedFeedId: string) => {
        history.push(selectedFeedId)
        // setShow(false)
    }

    return (
        <>
            <div className="collapse-container collapse-slider collapse-show">
                <FeedList />
            </div>
        </>
    )
}

export default CollapseMenu
