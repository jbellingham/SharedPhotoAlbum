import React from 'react'
import ProfilePicture, { IProfilePictureProps } from '../shared/ProfilePicture'
import { Row, Col } from 'react-bootstrap'

function Messages() {
    const profilePictureProps: IProfilePictureProps = {
        width: 50,
        height: 50,
    }
    return (
        <>
            <Row>
                <Col md={2}>
                    <ProfilePicture {...profilePictureProps} />
                    <span className="column-heading">Messages</span>
                </Col>
                <Col md={8}>
                    <ProfilePicture {...profilePictureProps} />
                    <span className="column-heading">Firstname Lastname</span>
                </Col>
            </Row>
            <Row>
                <Col md={{ span: 7, offset: 2 }}>
                    <div className="messages-container clearfix">
                        <div className="d-flex flex-row">
                            <p className="message-other">Poop</p>
                        </div>
                        <div className="message-self-container d-flex justify-content-end">
                            <p className="message-self">Poops. Some message here blah blah moo moo,...,.,</p>
                        </div>
                        <div className="d-flex flex-row">
                            <p className="message-other">Poop</p>
                        </div>
                        <div className="message-self-container d-flex flex-row-reverse">
                            <p className="message-self">Poops</p>
                        </div>
                    </div>
                </Col>
            </Row>
        </>
    )
}

export default Messages
