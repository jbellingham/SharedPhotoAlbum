import React from 'react'
import { Image } from 'react-bootstrap'
import profilePic from '../../../static/images/profile-pic.jpg'

export interface IProfilePictureProps {
    width: number
    height: number
}

function ProfilePicture(props: IProfilePictureProps) {
    return <Image alt="Jesse Bellingham" src={profilePic} roundedCircle width={props.width} height={props.height} />
}

export default ProfilePicture
