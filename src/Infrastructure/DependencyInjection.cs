using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Infrastructure.Identity;
using SharedPhotoAlbum.Infrastructure.Persistence;
using SharedPhotoAlbum.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddSignInManager<SignInManager>();
                
            var validIssuer = configuration["JwtOptions:Issuer"];
            
            services.AddAuthentication()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                    facebookOptions.Fields.Add("picture");
                    facebookOptions.ClaimActions.MapCustomJson(JwtClaimTypes.Picture,
                        json => json.GetProperty("picture").GetProperty("data")
                        .GetProperty("url").ToString());
                    facebookOptions.ClaimActions.MapCustomJson(JwtClaimTypes.GivenName, json => json.GetProperty("first_name").ToString());
                    facebookOptions.ClaimActions.MapCustomJson(JwtClaimTypes.FamilyName, json => json.GetProperty("last_name").ToString());
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
                {
                    jwtOptions.ClaimsIssuer = validIssuer;
                    jwtOptions.TokenValidationParameters = TokenValidation.BuildParameters(configuration);
                    jwtOptions.SaveToken = true;
                });

            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("ApiUser",
            //         policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol,
            //             Constants.Strings.JwtClaims.ApiAccess));
            // });

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton(configuration);

            return services;
        }
    }
}
