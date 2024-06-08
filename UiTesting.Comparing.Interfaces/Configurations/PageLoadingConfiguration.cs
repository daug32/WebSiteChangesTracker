namespace WebSiteComparer.Core.Configurations;

public class PageLoadingConfiguration
{
    /// <summary>
    /// If true, all java script at the pages will not be executed at all.<br/>
    /// Warning: it will break SPA sites, sliders, iframes and etc.<br/>
    /// Why to use: to guarantee that content at site is completely static.<br/> 
    /// </summary>
    public bool DisableJavaScript { get; set; } = false;
}