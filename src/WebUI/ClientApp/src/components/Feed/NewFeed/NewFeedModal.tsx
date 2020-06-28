import React from 'react'
import { Button, Form, FormControl, Container, Modal, ModalBody, ModalFooter } from 'react-bootstrap'
import { useStore } from '../../../stores/StoreContext'
import { CreateFeedCommand } from '../../../Client'
import { useHistory } from 'react-router-dom'

export interface IModalProps {
    show: boolean
    handleClose: () => void
}

function NewFeedModal(props: IModalProps): JSX.Element {
    const history = useHistory()
    const { show, handleClose } = props
    const [feedName, setFeedName] = React.useState('')
    const [feedDescription, setFeedDescription] = React.useState('')
    const [isSubmitting, setIsSubmitting] = React.useState(false)
    const { feedStore } = useStore()

    const feedNameInputId = 'feedNameInput'
    const feedDescriptionInputId = 'feedDescriptionInput'

    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>): void => {
        const { id, value } = event.currentTarget
        switch (id) {
            case feedNameInputId:
                setFeedName(value)
                break
            case feedDescriptionInputId:
                setFeedDescription(event.currentTarget.value)
                break
        }
    }

    const handleSubmit = async (): Promise<void> => {
        const feedId = await feedStore.createFeed(
            new CreateFeedCommand({ name: feedName, description: feedDescription }),
        )
        history.push(`/feed/${feedId}`)
        handleClose()
    }

    return (
        <Modal show={show} onHide={handleClose} onClose={handleClose}>
            <Modal.Header>Create a new feed</Modal.Header>
            <ModalBody>
                <Container fluid>
                    <Form>
                        <Form.Group>
                            <FormControl
                                id={feedNameInputId}
                                placeholder="Feed name"
                                value={feedName}
                                onChange={handleChange}
                            />
                        </Form.Group>
                        <Form.Group>
                            <FormControl
                                id={feedDescriptionInputId}
                                placeholder="What is this feed about?"
                                onChange={handleChange}
                                value={feedDescription}
                                as="textarea"
                            />
                        </Form.Group>
                    </Form>
                </Container>
            </ModalBody>
            <ModalFooter>
                <Button variant="primary" disabled={isSubmitting} onClick={handleSubmit}>
                    Create
                </Button>
            </ModalFooter>
        </Modal>
    )
}

export default NewFeedModal
