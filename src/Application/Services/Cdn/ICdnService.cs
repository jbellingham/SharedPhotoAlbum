using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using SharedPhotoAlbum.Domain.ValueObjects;

namespace SharedPhotoAlbum.Application.Services.Cdn
{
    public interface ICdnService
    {
        Task<FileUploadResult> UploadFiles(UploadRequest request);
    }

    public class FileUploadResult
    {
        public FileUploadResult()
        {
        }
        
        public List<UploadResult> UploadResults { get; set; } = new List<UploadResult>();

        public bool Success => UploadResults.All(_ => _.Success);

        public class UploadResult
        {
            public bool Success { get; set; }
            public File File { get; set; }
        }
    }
}