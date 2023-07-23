using WebSiteComparer.Core;

namespace WebSiteComparer.Console.Services;

public class ConsoleLogService : ILogService
{
    public void Message( string message )
    {
        System.Console.WriteLine( $"Message:\t{message}" );
    }

    public void Error( string message )
    {
        System.Console.WriteLine( $"Error:  \t{message}" );
    }
}