using System.Text;
using WebSiteComparer.Console.Commands;

namespace WebSiteComparer.Console.Utils;

public static class ArgumentsHandler
{
    private const string HelpCommand = "help";

    public static string GetHelp()
    {
        var builder = new StringBuilder();

        builder.AppendLine( "help -\tShow List of available commands" );
        builder.AppendLine( "get-screenshots -\tTake screenshots of all sites" );
        builder.AppendLine( "check-for-changes -\tTake screenshots of all sites and compare with previous versions" );

        return builder.ToString();
    }

    public static CommandType Parse( IEnumerable<string> args )
    {
        string? action = args.FirstOrDefault( arg => !string.IsNullOrWhiteSpace( arg ) );
        if ( action == null )
        {
            throw new ArgumentException( "No action type was specified" );
        }

        action = action.Trim().ToLower();

        return action switch
        {
            "get-screenshots" => CommandType.UpdateScreenshots,
            "check-for-changes" => CommandType.CheckForChanges,
            "help" => CommandType.NeedHelp,
            _ => throw new ArgumentException(
                $"Action wasn't recognized. Type \"{HelpCommand}\" to get list of available commands and their descriptions" )
        };
    }
}