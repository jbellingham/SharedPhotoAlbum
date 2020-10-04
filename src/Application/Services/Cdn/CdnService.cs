using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using SharedPhotoAlbum.Domain.Enums;
using SharedPhotoAlbum.Domain.ValueObjects;

namespace SharedPhotoAlbum.Application.Services.Cdn
{
    public class CdnService : ICdnService
    {
        private readonly ICloudinaryClient _client;
        private readonly string _environment;
        
        public CdnService(ICloudinaryClient client, IConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _environment = configuration["ASPNETCORE_ENVIRONMENT"]?.ToLower(CultureInfo.InvariantCulture) ?? "development";
        }
        
        public async Task<FileUploadResult> UploadFiles(UploadRequest request)
        {
            if (request.Files == null) throw new ArgumentNullException(nameof(request.Files));
            
            FileUploadResult uploadResult = await UploadAllFiles(request);
            return uploadResult;
        }

        // private async Task<FileUploadResult> UploadAllFiles(UploadRequest request)
        // {
        //     var uploadResult = new FileUploadResult();
        //     foreach (File file in request.Files)
        //     {
        //         await UploadFile(file, uploadResult);
        //     }
        //
        //     return uploadResult;
        // }

        private async Task<FileUploadResult> UploadAllFiles(UploadRequest request)
        {
            var folder = $"{_environment}/feed-{request.FeedId}";
            var uploadResult = new FileUploadResult();
            foreach (var file in request.Files)
            {
                if (file.FileType == FileType.Image)
                {
                    var result = await _client.UploadAsync(new ImageUploadParams
                    {
                        File = new FileDescription(file.DataUrl),
                        Folder = folder,
                        Transformation = new Transformation().Height(2000).Quality(60),
                        UploadPreset = "fcqdexto"
                    });
                    
                    file.PublicId = result.PublicId;
                    uploadResult.UploadResults.Add(new FileUploadResult.UploadResult
                    {
                        Success = result.StatusCode == HttpStatusCode.OK,
                        File = file
                    });
                }
                else
                {
                    var result = await _client.UploadAsync(new VideoUploadParams
                    {
                        File = new FileDescription(file.DataUrl),
                        Folder = folder,
                        Transformation = new Transformation().Height(2000).Quality(60),
                        UploadPreset = "fcqdexto"
                    });
                
                    file.PublicId = result.PublicId;
                    uploadResult.UploadResults.Add(new FileUploadResult.UploadResult
                    {
                        Success = result.StatusCode == HttpStatusCode.Accepted,
                        File = file
                    });
                    // uploadResult.VideoUploadResults.Add(result);
                }
            }

            return uploadResult;
        }
    }
}