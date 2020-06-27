import React from 'react'
import { Carousel, CarouselItem } from 'react-bootstrap'
import image from '../../../static/images/image-1.jpg'

function Showcase() {
    return (
        <div id="carousel-container">
            <Carousel>
                <CarouselItem className="showcase">
                    <img alt="" src={image} className="d-block w-100" />
                </CarouselItem>

                <CarouselItem className="showcase">
                    <img alt="" src={image} className="d-block w-100" />
                </CarouselItem>

                <CarouselItem className="showcase">
                    <img alt="" src={image} className="d-block w-100" />
                </CarouselItem>

                <CarouselItem className="showcase">
                    <img alt="" src={image} className="d-block w-100" />
                </CarouselItem>
            </Carousel>
        </div>
    )
}

export default Showcase
