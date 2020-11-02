import React from 'react'
import FeedList from '../../Feed/FeedList'
import { EventEmitter } from 'events'

export const collapseMenuEventEmitter = new EventEmitter()

function CollapseMenu() {
    return (
        <>
            <div className="collapse-container collapse-slider collapse-show">
                <FeedList />
            </div>
        </>
    )
}

export default CollapseMenu
