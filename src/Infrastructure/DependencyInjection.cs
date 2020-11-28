using System.IO;
using System.Security.Cryptography.X509Certificates;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Infrastructure.Identity;
using SharedPhotoAlbum.Infrastructure.Persistence;
using SharedPhotoAlbum.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("SharedPhotoAlbumDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var builder = services.AddIdentityServer(options => options.IssuerUri = configuration["JwtOptions:IssuerUri"])
                .AddAspNetIdentity<ApplicationUser>()
                .AddOperationalStore<ApplicationDbContext>()
                .AddIdentityResources()
                .AddApiResources()
                .AddClients();
            
            if (configuration.GetValue<bool>("UseDeveloperSigningCredential"))
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var bytes = File.ReadAllBytes($"/var/ssl/private/{configuration["WEBSITE_LOAD_CERTIFICATES"]}.p12");
                var certificate = new X509Certificate2(bytes);
                builder.AddSigningCredential(certificate);
            }

            services.AddAuthentication()
                .AddIdentityServerJwt()
                .AddFacebook(facebookOptions =>
                {
                    
                    facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                    facebookOptions.Fields.Add("picture");
                    facebookOptions.ClaimActions.MapCustomJson(CustomClaimTypes.Facebook.Picture,
                        json => json.GetProperty("picture").GetProperty("data")
                            .GetProperty("url").ToString());
                    facebookOptions.ClaimActions.MapJsonKey(CustomClaimTypes.Facebook.FirstName, "first_name");
                    facebookOptions.ClaimActions.MapJsonKey(CustomClaimTypes.Facebook.LastName, "last_name");
                    facebookOptions.ClaimActions.MapJsonKey(CustomClaimTypes.Facebook.ProviderKey, "id");
                });
            IdentityModelEventSource.ShowPII = true;

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton(configuration);

            return services;
        }
    }
}
