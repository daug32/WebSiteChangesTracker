namespace WebSiteComparer.Core
{
    public interface ILogService
    {
        void Message( string message );
        void Error( string message );
    }
}