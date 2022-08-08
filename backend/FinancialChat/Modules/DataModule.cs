using FinancialChat.Authentication;
using FinancialChat.Hubs;
using FinancialChat.Parameters;
using FinancialChat.Publisher;
using FinancialChat.Stock;

namespace FinancialChat.Modules
{
    public static class DataModule
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtParameters = configuration.GetSection("Jwt").Get<JwtParameters>();
            services.AddSingleton(jwtParameters);
            var rabbitParameters = configuration.GetSection("RabbitParameters").Get<RabbitParameters>();
            services.AddSingleton(rabbitParameters);
            var externalParameters = configuration.GetSection("ExternalServices").Get<ExternalServicesParameter>();
            services.AddSingleton(externalParameters);            

            services.AddSingleton<IDictionary<string, UserConnection>>(opts => new Dictionary<string, UserConnection>());
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IStockService, StockService>();

            return services;
        }
    }
}
