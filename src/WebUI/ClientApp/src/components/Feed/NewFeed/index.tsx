import React from 'react'
import { Button, Modal, Container, Form } from 'react-bootstrap'
import { useHistory } from 'react-router-dom'
import gql from 'graphql-tag'
import { useMutation } from 'react-apollo'

const CREATE_FEED = gql`
    mutation createFeed($name: String!, $description: String) {
        createFeed(name: $name, description: $description) {
            _id
        }
    }
`

interface IModalProps {
    show: boolean
    handleClose: () => void
}

function NewFeed(props: IModalProps): JSX.Element {
    const history = useHistory()
    const [feedName, setFeedName] = React.useState('')
    const [feedDescription, setFeedDescription] = React.useState('')
    const [createNewFeed] = useMutation(CREATE_FEED, {
        refetchQueries: ['feeds'],
        onCompleted({ createFeed }) {
            setTimeout(() => {
                props.handleClose()
                history.push(`/${createFeed._id}`)
            }, 500)
        },
    })

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
            await createNewFeed({
                variables: {
                    name: feedName,
                    description: feedDescription,
                },
            })
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
