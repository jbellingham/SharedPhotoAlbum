import React from 'react'
import { Button } from 'react-bootstrap'
import RequestModal, { IModalProps } from './RequestModal'

function Request() {
    const [show, setShow] = React.useState(false)

    const handleClickShow = () => {
        setShow(true)
    }

    const handleClose = () => {
        setShow(false)
    }

    const modalProps: IModalProps = {
        handleClose,
        show,
    }

    return (
        <>
            <Button variant="primary" onClick={handleClickShow}>
                Request commission
            </Button>
            <RequestModal {...modalProps} />
        </>
    )
}

export default Request
