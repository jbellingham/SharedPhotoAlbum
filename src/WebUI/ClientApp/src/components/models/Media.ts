export class Media {
    id?: string
    publicId?: string | undefined
    mimeType?: string | undefined
    postId?: string

    constructor(
        id: string | undefined,
        publicId: string | undefined,
        mimeType: string | undefined,
        postId: string | undefined,
    ) {
        this.id = id
        this.postId = postId
        this.publicId = publicId
        this.mimeType = mimeType
    }
}
