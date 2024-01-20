namespace Libs.Microsoft.Playwright.PageLoading;

public class PageLoadingOptions
{
    /// <summary>
    ///     If passed, app will wait until iframe is loaded
    /// </summary>
    public string? IframeCssSelector { get; set; } = null;

    /// <summary>
    ///     If true, app will wait until all scripts are executed<br/>
    ///     Default value is <b>true</b>
    /// </summary>
    public bool WaitUntilScriptsAreLoaded { get; set; } = true;

    /// <summary>
    ///     If passed, app will wait until these elements has completed animation 
    /// </summary>
    public List<string> WaitForTransitionEndAtElements { get; set; } = new();

    /// <summary>
    ///     If passed, app will surely wait this time
    /// </summary>
    public TimeSpan AdditionalLoadTime { get; set; } = TimeSpan.Zero;

    /// <summary>
    ///     After this amount of time loading completes with exception
    /// </summary>
    public int TimeoutInMilliseconds { get; set; } = 10 * 1000;
}