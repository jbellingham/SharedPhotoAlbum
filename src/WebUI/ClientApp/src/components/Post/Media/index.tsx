import React from 'react'
import { Carousel, CarouselItem } from 'react-bootstrap'
import { Image, Video } from 'cloudinary-react'
import { Media } from '../../models/Media'
import { useMediaQuery } from 'react-responsive'

export interface IMediaDto {
    onClick: () => void
    media: Media[] | undefined
}

const MediaContainer = (props: IMediaDto): JSX.Element => {
    const isDesktopOrLaptop = useMediaQuery({
        query: '(min-device-width: 1224px)',
    })

    const renderMedia = (media: Media): JSX.Element => {
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
                        width={isDesktopOrLaptop ? '' : '300'}
                        height={isDesktopOrLaptop ? '800' : ''}
                        crop="fit"
                        aspectRatio="16:9"
                    ></Image>
                )
        }
    }
    return (
        <Carousel interval={null} onClick={props.onClick}>
            {props.media?.map((_) => (
                <CarouselItem key={_.id}>
                    <div className="image-container d-flex justify-content-center">{renderMedia(_)}</div>
                </CarouselItem>
            ))}
        </Carousel>
    )
}

export default MediaContainer
