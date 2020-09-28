using System;
using SharedPhotoAlbum.Application.Common.Interfaces;
using SharedPhotoAlbum.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var id = Guid.Parse(userId);
            var user = await _userManager.Users.FirstAsync(u => u.Id == id);

            return user.UserName;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id.ToString());
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var id = Guid.Parse(userId);
            var user = _userManager.Users.SingleOrDefault(u => u.Id == id);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}
