namespace Libs.System.IO.Extensions;

public static class DirectoryInfoExtensions
{
    public static void ClearDirectory( this DirectoryInfo directoryInfo )
    {
        foreach ( FileInfo file in directoryInfo.GetFiles() )
        {
            file.Delete();
        }
    }
}