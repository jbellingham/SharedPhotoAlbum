using System;
using System.Text;
using IdentityModel;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Infrastructure.Identity;
using SharedPhotoAlbum.Infrastructure.Persistence;
using SharedPhotoAlbum.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Infrastructure
{
    public static class DependencyInjection
    {
        public const string CookieScheme = "CookieScheme";
        
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
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            
            // services.AddIdentityServer()
            //     .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            
            // JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var validIssuer = configuration["JwtOptions:Issuer"];
            var validAudience = configuration["JwtOptions:Audience"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtOptions:SigningKey"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = validIssuer,

                ValidateAudience = true,
                ValidAudience = validAudience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            services.AddAuthentication()
                // .AddIdentityServerJwt()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddFacebook(facebookOptions =>
                {
                    // facebookOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                    facebookOptions.Fields.Add("picture");
                    facebookOptions.ClaimActions.MapCustomJson(JwtClaimTypes.Picture,
                        json => json.GetProperty("picture").GetProperty("data")
                        .GetProperty("url").ToString());
                })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.ClaimsIssuer = validIssuer;
                    jwtOptions.TokenValidationParameters = tokenValidationParameters;
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
