using System.Reflection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using GamingApi.Dto;
using GamingApi.Integration;
using GamingApi.Patterns;
using GamingApi.Integration.Config;
using Yld.GamingApi.WebApi.Queries;

namespace Yld.GamingApi.WebApi;

public sealed class Startup
{
    private Assembly ExecutingAssembly => Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDefaultServices();

        ConfigureSettings(services);

        services.AddScoped<IQueryHandler<GetGameListQuery, GameListResponseDto>, GetGameListQueryHandler>();
        services.AddSingleton<IIntegrationService, IntegrationService>();
        services.AddHttpClient<IIntegrationService, IntegrationService>();

        ConfigureAutoMapper(services);
        ConfigureFluentValidation(services);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDefaultAppConfig();
    }

    private void ConfigureSettings(IServiceCollection services)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        services.Configure<IntegrationServiceSettings>(options => configuration.GetSection(nameof(IntegrationServiceSettings)).Bind(options));
    }

    private void ConfigureAutoMapper(IServiceCollection services)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(ExecutingAssembly);
            cfg.ShouldMapProperty = p => p.GetMethod?.IsPublic == true || p.GetMethod?.IsPrivate == true;
        });

        services.AddSingleton(config.CreateMapper());
    }
    private void ConfigureFluentValidation(IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<Startup>();
    }
}
