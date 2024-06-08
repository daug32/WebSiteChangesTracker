using System.IO;

namespace UiTesting.Comparing.Implementation.Extensions;

internal static class DirectoryInfoExtensions
{
    public static void ClearDirectory( this DirectoryInfo directoryInfo )
    {
        foreach ( FileInfo file in directoryInfo.GetFiles() )
        {
            file.Delete();
        }
    }
}