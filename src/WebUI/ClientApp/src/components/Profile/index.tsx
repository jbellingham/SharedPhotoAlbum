import React from 'react'
import { Row, Col } from 'react-bootstrap'
import Showcase from './Showcase'
import ProfilePicture, { IProfilePictureProps } from '../shared/ProfilePicture'

function Profile(): JSX.Element {
    const profilePictureProps: IProfilePictureProps = {
        height: 200,
        width: 200,
    }

    return (
        <>
            <Row>
                <Col xs={6} md={4}>
                    <ProfilePicture {...profilePictureProps} />
                </Col>
            </Row>
            <span className="user-name d-block">Jesse Bellingham</span>
            <Showcase />
        </>
    )
}

export default Profile
