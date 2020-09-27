import React from 'react'
import FeedList from '../../Feed/FeedList/FeedList'
import { useParams, useHistory } from 'react-router-dom'
import { EventEmitter } from 'events'

interface ICollapseMenuProps {
    children: any
}

export const collapseMenuEventEmitter = new EventEmitter()

function CollapseMenu(props: ICollapseMenuProps) {
    const [show, setShow] = React.useState(false)

    collapseMenuEventEmitter.on('toggle', () => {
        setShow(!show)
    })

    const { feedId } = useParams()
    const history = useHistory()

    const onFeedSelected = (selectedFeedId: string) => {
        history.push(selectedFeedId)
        setShow(false)
    }

    return (
        <>
            <div className={`collapse-container collapse-slider ${show ? 'collapse-show' : ''}`}>
                <FeedList onFeedSelected={onFeedSelected} selectedFeed={feedId} />
            </div>
            {props.children}
        </>
    )
}

export default CollapseMenu
