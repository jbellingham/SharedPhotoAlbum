import React from 'react'
import { Card, Form } from 'react-bootstrap'
import Comment from './Comment'
import MediaContainer from './MediaContainer'
import gql from 'graphql-tag'
import { useQuery, useMutation } from 'react-apollo'
import Zoom from './Zoom'

const GET_COMMENTS = gql`
    query($postId: String!) {
        comments(postId: $postId) {
            text
            likes
            commenter {
                _id
                email
                name
            }
        }
    }
`

const CREATE_COMMENT = gql`
    mutation createComment($text: String!, $postId: String!) {
        createComment(text: $text, postId: $postId) {
            _id
        }
    }
`

const Post = (props: any) => {
    const { post } = props
    const [comment, setComment] = React.useState('')
    const [showZoom, setShowZoom] = React.useState(false)

    const { data, loading } = useQuery(GET_COMMENTS, {
        variables: { postId: post._id },
    })

    const [createComment] = useMutation(CREATE_COMMENT, {
        onCompleted: () => setComment(''),
        refetchQueries: [
            {
                query: GET_COMMENTS,
                variables: { postId: post._id },
            },
        ],
    })

    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>): void => {
        setComment(event.currentTarget.value)
    }

    const onKeyDown = async (event: React.KeyboardEvent<HTMLInputElement>): Promise<void> => {
        if (event.key === 'Enter') {
            event.preventDefault()
            event.stopPropagation()
            if (comment && post._id) {
                createComment({ variables: { text: comment, postId: post._id } })
            }
        }
    }

    const onMediaClick = () => {
        setShowZoom(false) //!showZoom)
    }

    return (
        <Card className="post-container">
            {post.text && <Card.Header>{post.text}</Card.Header>}
            {post.media.length > 0 && (
                <Card.Body>
                    <MediaContainer media={post.media} onClick={onMediaClick} />
                    <Zoom media={post.media} show={showZoom} />
                </Card.Body>
            )}
            <Card.Body>
                {loading ? (
                    'Loading'
                ) : (
                    <div className="comments-container">
                        {data.comments.map((comment) => (
                            <Comment {...comment} key={comment._id} />
                        ))}
                    </div>
                )}
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
