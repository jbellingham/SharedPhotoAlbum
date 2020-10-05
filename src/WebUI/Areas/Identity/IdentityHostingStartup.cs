using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Infrastructure.Persistence;

[assembly: HostingStartup(typeof(SharedPhotoAlbum.WebUI.Areas.Identity.IdentityHostingStartup))]
namespace SharedPhotoAlbum.WebUI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}