import React, { ReactElement } from 'react'
import { ICommentDto } from '../../Client'
import ProfilePicture from '../shared/ProfilePicture'

function Comment(props: ICommentDto): ReactElement {
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
