﻿using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Infrastructure.Identity;
using SharedPhotoAlbum.Infrastructure.Persistence;
using SharedPhotoAlbum.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
                    options.UseNpgsql(
                        configuration.GetConnectionString("DefaultConnection"),
                        b =>
                        {
                            b.SetPostgresVersion(12, 2);
                            b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        }));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

                services.AddDefaultIdentity<ApplicationUser>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            
            services.AddAuthentication()
                .AddIdentityServerJwt()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                });

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }
    }
}
