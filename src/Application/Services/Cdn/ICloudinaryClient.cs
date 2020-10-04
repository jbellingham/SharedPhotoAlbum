using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace SharedPhotoAlbum.Application.Services.Cdn
{
    public interface ICloudinaryClient
    {
        Task<ImageUploadResult> UploadAsync(ImageUploadParams uploadParams);
        Task<VideoUploadResult> UploadAsync(VideoUploadParams uploadParams);
    }
}