import React from 'react'
import { Image } from 'react-bootstrap'
import { useStore } from '../../../stores/StoreContext'

function ProfilePicture() {
    const { userStore } = useStore()
    const { loading, profilePictureUrl } = userStore
    if (!loading) {
        // const { name, profilePicture } = data.getUser || {}
        return (
            <Image
                // alt={name}
                src={profilePictureUrl}
                roundedCircle
                width={100}//profilePicture?.width}
                height={100}//profilePicture?.height}
            />
        )
    }
    return null
}

export default ProfilePicture
