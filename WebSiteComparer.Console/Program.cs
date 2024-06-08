using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebSiteComparer.Console;

public class Program
{
    public static async Task Main( string[] args )
    {
        await BuildHost( args ).StartAsync( args );
    }

    private static WebSiteComparerApplication BuildHost( string[] args )
    {
        IHost host = Host
            .CreateDefaultBuilder( args )
            .ConfigureAppConfiguration( AddConfigurationFile )
            .ConfigureLogging( ConfigureLogging )
            .ConfigureServices( AddDependencies )
            .Build();

        return host.Services.GetRequiredService<WebSiteComparerApplication>();
    }

    private static void AddConfigurationFile( HostBuilderContext context, IConfigurationBuilder configurationBuilder )
    {
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        configurationBuilder
            .AddJsonFile( "appsettings.json" )
            .AddJsonFile( $"appsettings.{environment}.json", optional: true )
            .AddEnvironmentVariables();
    }

    private static void ConfigureLogging( HostBuilderContext context, ILoggingBuilder loggingBuilder )
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();
    }

    private static void AddDependencies( HostBuilderContext context, IServiceCollection services )
    {
        services.AddDependencies( context.Configuration );
    }
}