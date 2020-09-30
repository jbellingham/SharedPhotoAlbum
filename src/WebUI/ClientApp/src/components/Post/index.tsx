import React from 'react'
import { Card, Form } from 'react-bootstrap'
import Comment from './Comment'
import MediaContainer from './Media'
import Zoom from './Zoom'
import { IPostDto } from '../../Client'
import NewComment from './Comment/NewComment'
import CommentsList from './Comment/List'
import { observer } from 'mobx-react'

interface IPostProps {
    post: IPostDto
}

const Post = observer((props: IPostProps) => {
    const { post } = props
    const [showZoom, setShowZoom] = React.useState(false)

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
                <CommentsList postId={post.id} />
                {/* {loading ? (
                    'Loading'
                ) : (
                    <div className="comments-container">
                        {data.comments.map((comment) => (
                            <Comment {...comment} key={comment._id} />
                        ))}
                    </div>
                )} */}
                <NewComment postId={post.id} />
            </Card.Body>
        </Card>
    )
})

export default Post
