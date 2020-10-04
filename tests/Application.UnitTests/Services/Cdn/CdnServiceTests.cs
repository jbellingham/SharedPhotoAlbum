using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SharedPhotoAlbum.Application.Services.Cdn;
using SharedPhotoAlbum.Domain.Enums;
using SharedPhotoAlbum.Domain.ValueObjects;

namespace SharedPhotoAlbum.Application.UnitTests.Services.Cdn
{
    public class CdnServiceTests
    {
        private Mock<ICloudinaryClient> mockClient;
        private IConfiguration configuration = new ConfigurationRoot(new List<IConfigurationProvider>());

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<ICloudinaryClient>();
        }

        [Test]
        public async Task UploadFilesShouldReturnFileUploadResult()
        {
            var cdn = new CdnService(mockClient.Object, configuration);
            var result = await cdn.UploadFiles(new UploadRequest());
            result.Should().BeOfType<FileUploadResult>();
        }

        [Test]
        public async Task UploadFilesWithMultipleFilesShouldBeInResult()
        {
            mockClient.Setup(_ => _.UploadAsync(It.IsAny<ImageUploadParams>()))
                .Returns(Task.FromResult(new ImageUploadResult()));
            
            mockClient.Setup(_ => _.UploadAsync(It.IsAny<VideoUploadParams>()))
                .Returns(Task.FromResult(new VideoUploadResult()));
            
            var videoFile = File.For("data:video/mp4;dsadsafgf");
            var imageFile = File.For("data:image/jpg;dsadsafgf");
            var cdn = new CdnService(mockClient.Object, configuration);
            var result = await cdn.UploadFiles(new UploadRequest { Files = new List<File> { videoFile, imageFile }});
            result.UploadResults.Count(_ => _.File.FileType == FileType.Image).Should().Be(1);
            result.UploadResults.Count(_ => _.File.FileType == FileType.Video).Should().Be(1);
        }
    }
}