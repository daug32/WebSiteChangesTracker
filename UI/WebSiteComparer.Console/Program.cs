using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebSiteComparer.Console;

public class Program
{
    private static readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddJsonFile( "appsettings.json" )
        .AddEnvironmentVariables()
        .Build();
    
    public static async Task Main( string[] args )
    {
        await BuildHost( args ).StartAsync( args );
    }

    private static WebSiteComparerApplication BuildHost( string[] args )
    {
        return Host
            .CreateDefaultBuilder( args )
            .ConfigureServices( services => services.AddDependencies( _configuration ) )
            .Build()
            .Services
            .GetRequiredService<WebSiteComparerApplication>();
    }
}