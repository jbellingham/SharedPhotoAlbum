import React from 'react'
import { Modal, Container, Carousel } from 'react-bootstrap'
import { Image, Video } from 'cloudinary-react'
import { MediaModel } from '../../../api/media/media'

interface IZoomProps {
    show: boolean
    media: MediaModel[]
}

function Zoom(props: IZoomProps) {
    const renderMedia = (media: MediaModel) => {
        switch (media.mimeType) {
            case 'video/mp4':
            case 'video/mpeg':
            case 'video/ogg':
            case 'video/webm':
            case 'video/mov':
                return <Video publicId={media.publicId} controls width="800" height="800"></Video>
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
                        width="800"
                        height="800"
                    ></Image>
                )
        }
    }

    return (
        <Modal show={props.show} size="xl">
            {/* <Modal.Header closeButton>Create a new feed</Modal.Header> */}
            <Modal.Body>
                <Container fluid>
                    <Carousel interval={null}>
                        {props.media?.map((_) => (
                            <Carousel.Item key={_._id}>
                                <div className="image-container d-flex justify-content-center">{renderMedia(_)}</div>
                            </Carousel.Item>
                        ))}
                    </Carousel>
                </Container>
            </Modal.Body>
            <Modal.Footer></Modal.Footer>
        </Modal>
    )
}

export default Zoom
