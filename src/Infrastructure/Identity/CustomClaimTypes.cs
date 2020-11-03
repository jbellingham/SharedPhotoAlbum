namespace SharedPhotoAlbum.Infrastructure.Identity
{
    public class CustomClaimTypes
    {
        public class Facebook
        {
            public const string Picture = "urn:facebook:picture";
            public const string FirstName = "urn:facebook:firstname";
            public const string LastName = "urn:facebook:lastname";
            public const string ProviderKey = "urn:facebook:providerkey";
        }
    }
}