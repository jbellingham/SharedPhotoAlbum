import React from 'react'
import { Image } from 'react-bootstrap'
import { useQuery } from 'react-apollo'
// import { GET_USER } from '../../../../api/users/methods'

interface IProfilePictureProps {
    userId: string
}

function ProfilePicture(props: IProfilePictureProps) {
    const { data, loading } = useQuery(GET_USER, {
        variables: { userId: props.userId },
    })

    if (!loading) {
        const { name, profilePicture } = data.getUser || {}
        return (
            <Image
                alt={name}
                src={profilePicture?.url}
                roundedCircle
                width={profilePicture?.width}
                height={profilePicture?.height}
            />
        )
    }
    return null
}

export default ProfilePicture
