using System;
using System.Text;

namespace WebSiteComparer.Core.Extensions;

public static class UriExtensions
{
    public static string ToFilePath( this Uri uri )
    {
        StringBuilder result = new StringBuilder( uri.Host + uri.PathAndQuery + uri.Fragment )
            .Replace( '/', '_' )
            .Replace( '.', '_' )
            .Replace( '-', '_' );

        if ( result[^1] == '_' )
        {
            result = result.Remove( result.Length - 1, 1 );
        }

        return result.ToString();
    }
}