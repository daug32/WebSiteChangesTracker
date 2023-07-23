using System;
using System.Collections.Generic;

namespace WebSiteComparer.Core.WebPageProcessing.Models
{
    public class PageLoadConfiguration
    {
        /// <summary>
        ///     Optional parameter. If passed, app will wait until iframe is loaded
        /// </summary>
        public string IframeCssSelector { get; set; } = null;

        /// <summary>
        ///     Optional parameter. If passed, app will wait until all scripts are executed
        /// </summary>
        public bool WaitUntilScriptsAreLoaded { get; set; } = true;

        /// <summary>
        ///     Optional parameter. If passed, app will wait until this element has completed animation 
        /// </summary>
        public List<string> WaitForTransitionEndAtElements { get; set; } = new List<string>();

        /// <summary>
        ///     Optional parameter. If passed, app will surely wait this time
        /// </summary>
        public TimeSpan AdditionalLoadTime { get; set; } = TimeSpan.Zero;
    }
}