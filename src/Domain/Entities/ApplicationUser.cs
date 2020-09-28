using System;
using Microsoft.AspNetCore.Identity;

namespace SharedPhotoAlbum.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid> { }
    public class ApplicationRole : IdentityRole<Guid> { }
}
