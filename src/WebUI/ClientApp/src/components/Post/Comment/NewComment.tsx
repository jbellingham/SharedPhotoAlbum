import React from 'react'
import { Form } from 'react-bootstrap'
import { CreateCommentCommand } from '../../../Client'
import { useStore } from '../../../stores/StoreContext'

interface INewCommentProps {
    postId: string | undefined
}

function NewComment(props: INewCommentProps): JSX.Element {
    const [comment, setComment] = React.useState('')
    const { commentStore } = useStore()
    const { postId } = props

    const onKeyDown = async (event: React.KeyboardEvent<HTMLInputElement>): Promise<void> => {
        if (event.key === 'Enter') {
            event.preventDefault()
            event.stopPropagation()
            if (comment && postId) {
                await commentStore.createComment(new CreateCommentCommand({text: comment, postId: postId}))
            }
        }
    }

    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>): void => {
        setComment(event.currentTarget.value)
    }

    return <Form>
        <Form.Control
            placeholder="Write a comment..."
            value={comment}
            onKeyDown={onKeyDown}
            onChange={handleChange}
        />
    </Form>
}

export default NewComment