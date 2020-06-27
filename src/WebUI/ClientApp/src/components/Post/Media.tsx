import React from 'react'
import { IStoredMediaDto, MediaType } from '../../Client'
import { Card } from 'react-bootstrap'

const Media = (props: IStoredMediaDto): JSX.Element => {
    return (
        <>
            {props &&
                new Map([
                    [undefined, null],
                    [MediaType.Image, <Card.Img key={props.id} src={props.content}></Card.Img>],
                    [MediaType.Video, <video key={props.id} src={props.content} controls />],
                ]).get(props.mediaType)}
        </>
    )
}

export default Media
