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
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Domain.Entities;
using SharedPhotoAlbum.Infrastructure.Persistence;
using WebUI;

namespace SharedPhotoAlbum.Application.IntegrationTests
{
    [SetUpFixture]
    public class Testing
    {
        private static IConfigurationRoot _configuration;
        private static IServiceScopeFactory _scopeFactory;
        private static Checkpoint _checkpoint;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
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
                w.ApplicationName == "SharedPhotoAlbum.Api"));

            services.AddLogging();

            startup.ConfigureServices(services);

            // Setup testing user (need to add a user to identity and use a real guid)
            var currentUserServiceDescriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(ICurrentUserService));

            services.Remove(currentUserServiceDescriptor);

            services.AddTransient<ICurrentUserService, CurrentUserService>();

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        
            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new [] { "__EFMigrationsHistory" },
                SchemasToInclude = new []
                {
                    "public"
                },
                DbAdapter = DbAdapter.Postgres
            };

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
            public string UserId => _currentUserId;
        }

        private static string _currentUserId;

        public static async Task<string> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync("test@local", "Testing1234!");
        }

        public static async Task<string> RunAsUserAsync(string userName, string password)
        {
            using var scope = _scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser { UserName = userName, Email = userName };

            var result = await userManager.CreateAsync(user, password);

            _currentUserId = user.Id;

            return _currentUserId;
        }

        public static async Task ResetState()
        {
            await using (var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await conn.OpenAsync();
                await _checkpoint.Reset(conn);
                _currentUserId = null;
            }
        }

        public static async Task<T> FindAsync<T>(long id)
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
    }
}
