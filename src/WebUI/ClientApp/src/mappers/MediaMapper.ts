import { StoredMediaDto } from '../Client'
import { Media } from '../components/models/Media'

export class MediaMapper {
    static fromDto(storedMedia: StoredMediaDto): Media {
        return new Media(storedMedia.id, storedMedia.publicId, storedMedia.mimeType, storedMedia.postId)
    }
}
