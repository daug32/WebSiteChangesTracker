using System;

namespace WebSiteComparer.Core.WebPageProcessing.Implementation.Exceptions
{
    public class BrowserException : Exception
    {
        public BrowserException( string message )
            : base( message )
        {
        }
    }
}