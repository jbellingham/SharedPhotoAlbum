import React from 'react'
import { Card, Form } from 'react-bootstrap'
import MediaContainer from './Media'
import Zoom from './Zoom'
import NewComment from './Comment/NewComment'
import CommentsList from './Comment/List'
import { observer } from 'mobx-react'
import { Post as PostVm } from '../models/Post'

interface IPostProps {
    post: PostVm
}

const Post = observer((props: IPostProps) => {
    const { post } = props
    const { comments } = post
    const [showZoom, setShowZoom] = React.useState(false)

    const onMediaClick = () => {
        setShowZoom(false) //!showZoom)
    }

    return (
        <Card className="post-container">
            {post.text && <Card.Header>{post.text}</Card.Header>}
            {post.media.length > 0 && (
                <Card.Body>
                    <MediaContainer media={post.media} onClick={onMediaClick} />
                    {/* <Zoom media={post.storedMedia} show={showZoom} /> */}
                </Card.Body>
            )}
            <Card.Body>
                <CommentsList comments={comments} />
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
