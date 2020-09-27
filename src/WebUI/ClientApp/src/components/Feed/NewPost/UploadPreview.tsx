import React from 'react'
import { Image, Col } from 'react-bootstrap'

interface IUploadPreviewProps {
    files: string[]
}

function UploadPreview(props: IUploadPreviewProps) {
    return (
        <div className="d-flex justify-content-center">
            {props.files.map((data, index) => (
                <div key={`upload-preview-${index}`} className="thumbnail-container">
                    <Image src={data} thumbnail />
                </div>
            ))}
        </div>
    )
}

export default UploadPreview
