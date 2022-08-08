using FinancialChat.Authentication;
using FinancialChat.Hubs;
using FinancialChat.Parameters;

namespace FinancialChat.Modules
{
    public static class DataModule
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            var clientParameters = configuration.GetSection("Jwt").Get<JwtParameters>();
            services.AddSingleton(clientParameters);

            services.AddSingleton<IDictionary<string, UserConnection>>(opts => new Dictionary<string, UserConnection>());
            services.AddSingleton<ITokenService, TokenService>();

            return services;
        }
    }
}
