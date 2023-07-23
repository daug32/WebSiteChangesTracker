using System.Collections.Generic;
using System.Linq;

namespace WebSiteComparer.Core.WebPageProcessing.Implementation.Dictionaries
{
    internal static class JavaScriptLibs
    {
        public static string WaitForTransitionEndAtElement( IEnumerable<string> selectors )
        {
            string serializedSelectors = selectors.Aggregate("", (result, el) => $"'{el}', {result}" );
            return $@"
(function(){{
if (!document.transitionWaiter) {{
    let selectors = [{serializedSelectors}];
    document.transitionWaiter = selectors.map(el => ({{
        selector: el,
        status: false,
        isInitialized: false
    }}) );
}}

let hasNotInitialized = false;

for (let i = 0; i < document.transitionWaiter.length; i++) {{
    let waiter = document.transitionWaiter[i];
    if (waiter.isInitialized) {{
        continue;
    }}

    let el = document.querySelector(waiter.selector);
    if (!el) {{
        hasNotInitialized = true;
        continue;
    }}

    el.addEventListener('transitionend', (e) => waiter.status = true);
    waiter.isInitialized = true;
}}

return !hasNotInitialized && document.transitionWaiter.every(waiter => waiter.status);
}})();";
        }

        public static string IsDocumentReady()
        {
            return "(function(){if(document.readyState == 'complete'){return 1}})()";
        }
    }
}