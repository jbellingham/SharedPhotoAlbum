import React from 'react'
import Comment from '../index'
import { useStore } from '../../../../stores/StoreContext'
import { observer } from 'mobx-react'

interface ICommentsListProps {
    postId: string | undefined
}

const CommentsList = observer((props: ICommentsListProps) => {
    const { commentStore } = useStore()
    const comments = commentStore.getComments(props.postId)
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
})

export default CommentsList
