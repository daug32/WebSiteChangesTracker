using System.Text;
using WebSiteComparer.Console.Commands;

namespace WebSiteComparer.Console.Utils;

internal static class ArgumentsHandler
{
    public static string GetHelp()
    {
        var builder = new StringBuilder();

        builder.AppendLine( "help - Show List of available commands" );
        builder.AppendLine( "get-screenshots - Take screenshots of all sites" );
        builder.AppendLine( "check-for-changes - Take screenshots of all sites and compare with previous versions" );

        return builder.ToString();
    }

    public static CommandType Parse( IEnumerable<string> args )
    {
        string action = args.FirstOrDefault( arg => !string.IsNullOrWhiteSpace( arg ) ) ?? String.Empty;
        action = action.Trim().ToLower();

        return action switch
        {
            "get-screenshots" => CommandType.UpdateScreenshots,
            "check-for-changes" => CommandType.CheckForChanges,
            "help" => CommandType.NeedHelp,
            _ => throw new ArgumentOutOfRangeException( nameof( action ), action )
        };
    }
}