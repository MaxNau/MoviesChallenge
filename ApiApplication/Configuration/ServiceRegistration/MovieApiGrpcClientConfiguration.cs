using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProtoDefinitions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiApplication.Configuration.ServiceRegistration
{
    public static class MovieApiGrpcClientConfiguration
    {
        /// <summary>
        /// Adds and configures Grpc client for the Movies API
        /// </summary>
        /// <param name="services"></param>
        public static void Configure(IServiceCollection services, bool isDevelopment)
        {
            services
                .AddGrpcClient<MoviesApi.MoviesApiClient>((serviceProvider, options) =>
                {
                    var moviesApiConfig = serviceProvider.GetService<IOptions<MoviesApiConfig>>().Value;
                    options.Address = new Uri(moviesApiConfig.Address);
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var httpHandler = new HttpClientHandler();

                    if (isDevelopment)
                    {
                        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    }

                    return httpHandler;
                })
                .ConfigureChannel((serviceProvider, options) =>
                {
                    var moviesApiConfig = serviceProvider.GetService<IOptions<MoviesApiConfig>>().Value;
                    var credentials = CallCredentials.FromInterceptor((context, metadata) =>
                    {
                        metadata.Add("X-Apikey", moviesApiConfig.ApiKey);
                        return Task.CompletedTask;
                    });

                    options.Credentials = ChannelCredentials.Create(new SslCredentials(), credentials);
                });
        }
    }
}
