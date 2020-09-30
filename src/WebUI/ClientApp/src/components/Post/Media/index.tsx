import React from 'react'
import { Carousel, CarouselItem } from 'react-bootstrap'
import { Image, Video } from 'cloudinary-react'
import { MediaModel } from '../../../api/media/media'

export interface IMediaDto {
    onClick: () => void
    media: MediaModel[]
}

const MediaContainer = (props: IMediaDto): JSX.Element => {
    const renderMedia = (media: MediaModel) => {
        switch (media.mimeType) {
            case 'video/mp4':
            case 'video/mpeg':
            case 'video/ogg':
            case 'video/webm':
            case 'video/mov':
                return <Video publicId={media.publicId} controls width="500" height="500"></Video>
            case 'image/jpg':
            case 'image/jpeg':
            default:
                return (
                    <Image
                        publicId={media.publicId}
                        dpr="auto"
                        responsive
                        fetchFormat="auto"
                        quality="auto"
                        width="500"
                        height="500"
                    ></Image>
                )
        }
    }
    return (
        <Carousel interval={null} onClick={props.onClick}>
            {props.media?.map((_) => (
                <CarouselItem key={_._id}>
                    <div className="image-container d-flex justify-content-center">{renderMedia(_)}</div>
                </CarouselItem>
            ))}
        </Carousel>
    )
}

export default MediaContainer
