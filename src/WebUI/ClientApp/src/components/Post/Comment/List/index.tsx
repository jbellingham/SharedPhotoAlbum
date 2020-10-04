import React from 'react'
import Comment from '../index'
import { Comment as CommentVm } from '../../../models/Comment'
import { observer } from 'mobx-react'

interface ICommentsListProps {
    comments: CommentVm[]
    // postId: string | undefined
}

const CommentsList = observer(
    (props: ICommentsListProps): JSX.Element => {
        // const { commentStore } = useStore()
        const { comments } = props
        return (
            <>
                {comments?.length > 0 ? (
                    <div className="comments-container">
                        {comments.map((comment) => (
                            <Comment {...comment} key={comment.id} />
                        ))}
                    </div>
                ) : null}
            </>
        )
    },
)

export default CommentsList
