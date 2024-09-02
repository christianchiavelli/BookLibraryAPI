using BookLibraryAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryAPI.Extensions.Database
{
    public static class DatabaseExtensions
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
