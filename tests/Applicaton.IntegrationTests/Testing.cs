using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Npgsql;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Application.IntegrationTests.Seeds;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Infrastructure.Persistence;
using WebUI;

namespace SharedPhotoAlbum.Application.IntegrationTests
{
    [SetUpFixture]
    public class Testing
    {
        private static IConfiguration _configuration;
        private static IServiceScopeFactory _scopeFactory;
        private static Respawner _checkpoint;

        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var startup = new Startup(_configuration);

            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "SharedPhotoAlbum.WebUI"));

            services.AddLogging();

            startup.ConfigureServices(services);

            // Setup testing user (need to add a user to identity and use a real guid)
            var currentUserServiceDescriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(ICurrentUserService));

            services.Remove(currentUserServiceDescriptor);

            services.AddTransient<ICurrentUserService, CurrentUserService>();

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
            _checkpoint = await Respawner.CreateAsync(_configuration.GetConnectionString("DefaultConnection"),
                new RespawnerOptions
                {
                    TablesToIgnore = new []{ new Table("__EFMigrationsHistory")},
                    SchemasToInclude = new[]
                    {
                        "public"
                    },
                    DbAdapter = DbAdapter.Postgres
                });

            EnsureDatabase();
        }

        private static void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.Migrate();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        private class CurrentUserService : ICurrentUserService
        {
            public Guid UserId => _currentUserId;
        }

        private static Guid _currentUserId;

        public static async Task<Guid> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync(UserSeed.DefaultUser);
        }

        private static async Task<Guid> RunAsUserAsync(ApplicationUser user)
        {
            using var scope = _scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            var userResult = await userManager.FindByEmailAsync(user.UserName);
            if (userResult == null)
            {
                await userManager.CreateAsync(user, UserSeed.DefaultPassword);
            }

            _currentUserId = user.Id;

            return _currentUserId;
        }

        public static async Task ResetState()
        {
            await using (var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await conn.OpenAsync();
                await _checkpoint.ResetAsync(conn);
                _currentUserId = Guid.Empty;
            }
        }

        public static async Task<T> FindAsync<T>(Guid id)
            where T : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await context.FindAsync<T>(id);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }

        public static async Task SeedDatabase()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var userId = await RunAsDefaultUserAsync();
            await FeedSeed.Seed(db, userId);
            await PostSeed.Seed(db, userId);
        }
    }
}
