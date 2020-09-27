import React from 'react'
import { useParams } from 'react-router-dom'
import { Button, Row, Col } from 'react-bootstrap'
import gql from 'graphql-tag'
import { useMutation, useQuery } from 'react-apollo'

const CREATE_SUBSCRIPTION = gql`
    mutation createSubscription($feedId: String!, $userId: String!) {
        createSubscription(feedId: $feedId, userId: $userId) {
            _id
        }
    }
`

const GET_FEED = gql`
    query feedByInviteCode($inviteCode: String!) {
        feedByInviteCode(inviteCode: $inviteCode) {
            _id
            isActiveSubscription
        }
    }
`

function Invite() {
    const { inviteCode } = useParams()
    const [userId] = React.useState()//Meteor.userId())
    const [requested, setRequested] = React.useState(false)

    const { data, loading } = useQuery(GET_FEED, {
        variables: { inviteCode },
        onCompleted({ feedByInviteCode }) {
            setRequested(feedByInviteCode.isActiveSubscription)
        },
    })

    const { feedByInviteCode: feed } = data || {}

    const [createSubscription] = useMutation(CREATE_SUBSCRIPTION, {
        onCompleted() {
            setRequested(true)
        },
        onError(e) {
            debugger
        },
    })

    const acceptInvite = () => {
        if (feed?._id && userId) {
            createSubscription({ variables: { feedId: feed._id, userId } })
        }
    }

    return (
        <>
            {userId && (
                <Row>
                    <Col md={{ span: 2, offset: 4 }}>
                        {loading ? (
                            'loading'
                        ) : (
                            <>
                                {requested ? (
                                    <span>Subscription requested</span>
                                ) : (
                                    <Button variant="primary" onClick={acceptInvite}>
                                        Request subscription
                                    </Button>
                                )}
                            </>
                        )}
                    </Col>
                </Row>
            )}
        </>
    )
}

export default Invite
