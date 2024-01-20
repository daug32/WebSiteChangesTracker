using Microsoft.Playwright;

namespace Libs.Microsoft.Playwright.Factories;

public class PagesHandler
{
    private readonly IBrowser _browser;
    private readonly HashSet<IPage> _pages;

    public int MaxPageCount { get; }

    public PagesHandler( IBrowser browser, int maxPageCount = 10 )
    {
        _browser = browser;
        MaxPageCount = maxPageCount;
        _pages = new HashSet<IPage>( maxPageCount );
    }

    public async Task<IPage> CreateAsync()
    {
        if ( _pages.Count < MaxPageCount )
        {
            IPage newPage = await _browser.NewPageAsync();
            _pages.Add( newPage );
            return newPage;
        }
        
        return _pages.Last();
    }

    public Task CloseAsync( IPage page )
    {
        if ( !_pages.Contains( page ) )
        {
            throw new ArgumentException();
        }

        _pages.Remove( page );

        return page.CloseAsync();
    }
}