using BookLibraryAPI.Data;
using Microsoft.AspNetCore.Identity;

namespace BookLibraryAPI.Extensions.Authentication
{
    public static class IdentityExtensions
    {
        public static void AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
