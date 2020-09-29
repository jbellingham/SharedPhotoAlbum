using SharedPhotoAlbum.Domain.Entities;

namespace SharedPhotoAlbum.Application.IntegrationTests.Seeds
{
    public class UserSeed
    {
        public static ApplicationUser DefaultUser = new ApplicationUser
        {
            UserName = "default@testing", Email = "default@testing"
        };

        public static string DefaultPassword = "Testing1234!";
    }
}