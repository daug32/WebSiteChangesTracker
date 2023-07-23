using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebSiteComparer.Console;

public class Program
{
    public static async Task Main( string[] args )
    {
        await BuildHost( args ).StartAsync( args );
    }

    private static WebSiteComparerApplication BuildHost( string[] args )
    {
        return Host
            .CreateDefaultBuilder( args )
            .ConfigureServices( services => services.AddDependencies( AddConfigurationFile()) )
            .Build()
            .Services
            .GetRequiredService<WebSiteComparerApplication>();
    }

    private static IConfigurationRoot AddConfigurationFile()
    {
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        return new ConfigurationBuilder()
            .AddJsonFile( "appsettings.json" )
            .AddJsonFile( $"appsettings.{environment}.json", optional: true )
            .AddEnvironmentVariables()
            .Build();
    }
}