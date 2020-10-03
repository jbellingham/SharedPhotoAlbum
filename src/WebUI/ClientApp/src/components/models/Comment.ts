export class Comment {
    id?: string
    text?: string | undefined
    likes?: number
    postId?: string

    constructor(id: string, text: string, likes: number, postId: string) {
        this.id = id
        this.text = text
        this.likes = likes
        this.postId = postId
    }
}
