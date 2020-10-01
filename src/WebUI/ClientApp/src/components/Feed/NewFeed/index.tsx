import React from 'react'
import { Button, Modal, Container, Form } from 'react-bootstrap'
import { useHistory } from 'react-router-dom'
import { CreateFeedCommand } from '../../../Client'
import { useStore } from '../../../stores/StoreContext'

interface IModalProps {
    show: boolean
    handleClose: () => void
}

function NewFeed(props: IModalProps): JSX.Element {
    const history = useHistory()
    const [feedName, setFeedName] = React.useState('')
    const [feedDescription, setFeedDescription] = React.useState('')
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
        if (feedName) {
            const feedId = await feedStore.createFeed(
                new CreateFeedCommand({ name: feedName, description: feedDescription }),
            )
            setTimeout(() => {
                props.handleClose()
                history.push(`/${feedId}`)
            }, 500)
            setFeedName('')
            setFeedDescription('')
        }
    }

    return (
        <>
            <Modal show={props.show} onHide={props.handleClose} onClose={props.handleClose}>
                <Modal.Header closeButton>Create a new feed</Modal.Header>
                <Modal.Body>
                    <Container fluid>
                        <Form>
                            <Form.Group>
                                <Form.Control
                                    id={feedNameInputId}
                                    placeholder="Feed name"
                                    value={feedName}
                                    onChange={handleChange}
                                />
                            </Form.Group>
                            <Form.Group>
                                <Form.Control
                                    id={feedDescriptionInputId}
                                    placeholder="What is this feed about?"
                                    onChange={handleChange}
                                    value={feedDescription}
                                    as="textarea"
                                />
                            </Form.Group>
                        </Form>
                    </Container>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={handleSubmit}>
                        Create
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    )
}

export default NewFeed
