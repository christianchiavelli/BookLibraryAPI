namespace BookLibraryAPI.Extensions.HealthChecks
{
    public static class HealthChecksExtensions
    {
        public static void AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks();
        }
    }
}
