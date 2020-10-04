using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace SharedPhotoAlbum.Application.Services.Cdn
{
    public class CloudinaryClient : ICloudinaryClient
    {
        private readonly Cloudinary _client;
        public CloudinaryClient(IConfiguration configuration)
        {
            var cloudUrl = configuration["Cloudinary:CloudUrl"];
            _client = new Cloudinary(cloudUrl);
        }
        
        public virtual async Task<ImageUploadResult> UploadAsync(ImageUploadParams uploadParams)
        {
            return await _client.UploadAsync(uploadParams);
        }

        public virtual async Task<VideoUploadResult> UploadAsync(VideoUploadParams uploadParams)
        {
            return await _client.UploadAsync(uploadParams);
        }
    }
}