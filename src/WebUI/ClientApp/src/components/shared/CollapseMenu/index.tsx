import React from 'react'
import FeedList from '../../Feed/FeedList'
import { useParams, useHistory } from 'react-router-dom'
import { EventEmitter } from 'events'

interface ICollapseMenuProps {
    children: any
}

export const collapseMenuEventEmitter = new EventEmitter()

function CollapseMenu(props: ICollapseMenuProps) {
    // const [show, setShow] = React.useState(false)

    // collapseMenuEventEmitter.on('toggle', () => {
    //     setShow(!show)
    // })

    const history = useHistory()

    const onFeedSelected = (selectedFeedId: string) => {
        history.push(selectedFeedId)
        // setShow(false)
    }

    return (
        <>
            <div className='collapse-container collapse-slider collapse-show'>
                <FeedList />
            </div>
            {props.children}
        </>
    )
}

export default CollapseMenu
