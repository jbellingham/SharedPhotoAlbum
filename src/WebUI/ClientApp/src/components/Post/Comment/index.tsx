import React, { ReactElement } from 'react'
import { Comment as CommentModel } from '../../models/Comment'
import ProfilePicture from '../../shared/ProfilePicture'

function Comment(props: CommentModel): ReactElement {
    const { text } = props
    return (
        <div className="comment">
            <div className="d-flex flex-row">
                <div className="comment-profile-picture-container">
                    {/* <ProfilePicture userId={props.commenter.id} /> */}
                </div>
                <div className="comment-body">
                    {/* <span className="commenter-name">{props.commenter.name || props.commenter.email}</span> */}
                    <span>
                        <br />
                        {text}
                    </span>
                </div>
                <br />
            </div>
        </div>
    )
}

export default Comment
