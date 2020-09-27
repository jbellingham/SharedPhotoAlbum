import React from 'react'
import { useParams } from 'react-router-dom'
import gql from 'graphql-tag'
import { useQuery, useMutation } from 'react-apollo'
import { Button } from 'react-bootstrap'

const GET_PENDING_SUBSCRIPTIONS = gql`
    query pendingSubscriptionsByFeedId($feedId: String!) {
        pendingSubscriptionsByFeedId(feedId: $feedId) {
            _id
            user {
                _id
                email
                name
            }
        }
    }
`

const UPDATE_SUBSCRIPTION = gql`
    mutation updateSubscription($subscriptionId: String!, $newValue: Boolean!) {
        updateSubscription(subscriptionId: $subscriptionId, newValue: $newValue) {
            _id
        }
    }
`

function SubscriptionRequests() {
    const { feedId } = useParams()
    const { data, loading } = useQuery(GET_PENDING_SUBSCRIPTIONS, {
        variables: { feedId },
    })

    const [updateSubscription] = useMutation(UPDATE_SUBSCRIPTION, {
        refetchQueries: [
            {
                query: GET_PENDING_SUBSCRIPTIONS,
                variables: { feedId },
            },
        ],
    })

    const acceptRequest = async (subscriptionId: string) => {
        await updateSubscription({ variables: { subscriptionId, newValue: true } })
    }

    const buildUserComponent = (request: any) => {
        return (
            <>
                {request.user.name}
                <Button className="ml-2" variant="primary" onClick={() => acceptRequest(request._id)}>
                    Accept
                </Button>
            </>
        )
    }

    const { pendingSubscriptionsByFeedId: requests } = data || {}

    return (
        <div className="subscription-requests panel">
            <h5>Pending subscription requests</h5>
            {loading && 'loading'}
            {!loading && requests.length > 0
                ? requests.map((request) => buildUserComponent(request))
                : 'No pending requests'}
        </div>
    )
}

export default SubscriptionRequests
