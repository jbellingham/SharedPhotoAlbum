import React from 'react'
import { Button } from 'react-bootstrap'
import { IModalProps } from './NewFeedModal'
import NewFeedModal from './NewFeedModal'

function NewFeed(): JSX.Element {
    const [show, setShow] = React.useState(false)

    const handleClickShow = (): void => {
        setShow(true)
    }

    const handleClose = (): void => {
        setShow(false)
    }

    const modalProps: IModalProps = {
        handleClose,
        show,
    }

    return (
        <>
            <Button variant="primary" onClick={handleClickShow}>
                Create new feed
            </Button>
            <NewFeedModal {...modalProps} />
        </>
    )
}

export default NewFeed
