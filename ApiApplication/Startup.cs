using ApiApplication.Cache;
using ApiApplication.Configuration;
using ApiApplication.Configuration.ServiceRegistration;
using ApiApplication.Contracts;
using ApiApplication.Database;
using ApiApplication.Database.Repositories;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Filters.Exception;
using ApiApplication.Serializers;
using ApiApplication.Services;
using ApiApplication.Services.Abstractions;
using ApiApplication.Services.ApiClients;
using ApiApplication.Versioning;
using Asp.Versioning;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ProtoBuf.Meta;
using StackExchange.Redis;
using System.Reflection;

namespace ApiApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMapper(services);

            RuntimeTypeModel.Default.Add(typeof(Show))
                .Add(nameof(Show.FullTitle))
                .Add(nameof(Show.Title))
                .Add(nameof(Show.Crew))
                .Add(nameof(Show.Id))
                .Add(nameof(Show.Rank))
                .Add(nameof(Show.Image))
                .Add(nameof(Show.ImDbRating))
                .Add(nameof(Show.ImDbRatingCount))
                .Add(nameof(Show.Year));

            RuntimeTypeModel.Default.CompileInPlace();

            services.Configure<MoviesApiConfig>(Configuration.GetSection(nameof(MoviesApiConfig)));
            services.Configure<RedisConfig>(Configuration.GetSection(nameof(RedisConfig)));
            services.Configure<ReservationConfig>(Configuration.GetSection(nameof(ReservationConfig)));

            services.AddTransient<ValidationExceptionFilterAttribute>();

            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var redisConfig = serviceProvider.GetService<IOptions<RedisConfig>>().Value;
                return ConnectionMultiplexer.Connect(redisConfig.Address);
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Cinema API",
                    Description = "The API provides functionality to manage the showtimes of the cinema"
                });
                options.DocumentFilter<ReplaceVersionFilter>();
                options.OperationFilter<VersionHeaderFilter>();
            });

            services.AddTransient<IShowtimesRepository, ShowtimesRepository>();
            services.AddTransient<ITicketsRepository, TicketsRepository>();
            services.AddTransient<IAuditoriumsRepository, AuditoriumsRepository>();
            services.AddTransient<IReservationsRepository, ReservationsRepository>();

            services.AddSingleton<ISerializer, ProtobufSerializer>();
            services.AddSingleton<ICache, RedisCache>();

            services.AddTransient<IShowtimesService, ShowtimesService>();
            services.AddTransient<IReservationsService, ReservationsService>();
            services.AddTransient<ITicketsService, TicketsService>();

            MovieApiGrpcClientConfiguration.Configure(services, HostEnvironment.IsDevelopment());

            services.AddSingleton<IMoviesApiClient, MoviesApiGrpcClient>();
            services.AddSingleton<IMoviesApiService, MoviesApiService>();

            services.AddDbContext<CinemaContext>(options =>
            {
                options.UseInMemoryDatabase("CinemaDb")
                    .EnableSensitiveDataLogging()
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddControllers();
            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                })
                .AddApiExplorer(options =>
                {
                    options.SubstituteApiVersionInUrl = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "Cinema API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SampleData.Initialize(app);
        }

        private void ConfigureMapper(IServiceCollection services)
        {
            var globalSettings = TypeAdapterConfig.GlobalSettings;
            globalSettings.Scan(Assembly.GetExecutingAssembly());
            var mapperConfig = new Mapper(globalSettings);
            services.AddSingleton<IMapper>(mapperConfig);
        }
    }
}
