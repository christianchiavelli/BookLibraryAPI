namespace BookLibraryAPI.Extensions.Routing
{
    public static class RouteExtensions
    {
        public static void AddLowerCaseUrls(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
        }
    }
}
