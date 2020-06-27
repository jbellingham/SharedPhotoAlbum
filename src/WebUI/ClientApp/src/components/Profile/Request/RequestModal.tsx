import React from 'react'
import { Button, Form, FormControl, Container, Modal, ModalBody, ModalFooter } from 'react-bootstrap'

export interface IModalProps {
    show: boolean
    handleClose: () => void
}

function RequestModal(props: IModalProps) {
    const { show, handleClose } = props
    const [message, setMessage] = React.useState('')
    const [isSubmitting, setIsSubmitting] = React.useState(false)

    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setMessage(event.currentTarget.value)
    }

    const handleSubmit = () => {
        setTimeout(() => {
            alert(JSON.stringify(message, null, 2))
            setIsSubmitting(false)
        })
    }

    return (
        <Modal show={show} onHide={handleClose} onClose={handleClose}>
            <ModalBody>
                <Container fluid>
                    <Form>
                        <FormControl placeholder="Message" value={message} onChange={handleChange} />
                    </Form>
                </Container>
            </ModalBody>
            <ModalFooter>
                <Button variant="primary" disabled={isSubmitting} onClick={handleSubmit}>
                    Submit
                </Button>
            </ModalFooter>
        </Modal>
    )
}

export default RequestModal
