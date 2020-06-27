import React, { ReactElement } from 'react'
import { ICommentDto } from '../../Client'

function Comment(props: ICommentDto): ReactElement {
    return (
        <div className="d-flex flex-row">
            <p className="comment">
                Likes:{props.likes}
                <br />
                {props.text}
            </p>
        </div>
    )
}

export default Comment
