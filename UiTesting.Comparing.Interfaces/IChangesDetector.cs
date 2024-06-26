﻿using WebSiteComparer.Core.Configurations;

namespace WebSiteComparer.Core.ChangesTracking;

public interface IChangesDetector
{
    Task FindChangesAsync( WebsiteConfiguration configuration );
    Task FindChangesAsync( IEnumerable<WebsiteConfiguration> configuration );
}