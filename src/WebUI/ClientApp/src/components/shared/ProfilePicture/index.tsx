import { observer } from 'mobx-react'
import React from 'react'
import { Image } from 'react-bootstrap'
import { useStore } from '../../../stores/StoreContext'

const ProfilePicture = observer(() => {
    const { userStore } = useStore()
    const { user } = userStore
    if (!user) {
        userStore.getUserProfile()
    }
    if (!userStore.loading) {
        return (
            <Image
                alt={`${user?.firstName} ${user?.lastName}`}
                src={user?.profilePictureUrl}
                roundedCircle
                width={50}
                height={50}
            />
        )
    }
    return null
})

export default ProfilePicture
