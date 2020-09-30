import React from 'react'
import { Card, Form } from 'react-bootstrap'
import Comment from './Comment'
import MediaContainer from './Media'
import Zoom from './Zoom'
import { CreateCommentCommand, IPostDto } from '../../Client'
import { useStore } from '../../stores/StoreContext'

interface IPostProps {
    post: IPostDto
}

const Post = (props: IPostProps) => {
    const { post } = props
    const [comment, setComment] = React.useState('')
    const [showZoom, setShowZoom] = React.useState(false)
    const { commentStore } = useStore()

    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>): void => {
        setComment(event.currentTarget.value)
    }

    const onKeyDown = async (event: React.KeyboardEvent<HTMLInputElement>): Promise<void> => {
        if (event.key === 'Enter') {
            event.preventDefault()
            event.stopPropagation()
            if (comment && post.id) {
                await commentStore.createComment(new CreateCommentCommand({text: comment, postId: post.id}))
            }
        }
    }

    const onMediaClick = () => {
        setShowZoom(false) //!showZoom)
    }

    const { comments } = post

    return (
        <Card className="post-container">
            {post.text && <Card.Header>{post.text}</Card.Header>}
            {/* {post.media.length > 0 && (
                <Card.Body>
                    <MediaContainer media={post.media} onClick={onMediaClick} />
                    <Zoom media={post.media} show={showZoom} />
                </Card.Body>
            )} */}
            <Card.Body>
                {comments?.length > 0 &&
                    <div className="comments-container">
                        {comments.map((comment) => (
                            <Comment {...comment} key={comment.id} />
                        ))}
                    </div>}
                {/* {loading ? (
                    'Loading'
                ) : (
                    <div className="comments-container">
                        {data.comments.map((comment) => (
                            <Comment {...comment} key={comment._id} />
                        ))}
                    </div>
                )} */}
                <Form>
                    <Form.Control
                        placeholder="Write a comment..."
                        value={comment}
                        onKeyDown={onKeyDown}
                        onChange={handleChange}
                    />
                </Form>
            </Card.Body>
        </Card>
    )
}

export default Post
