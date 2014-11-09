namespace CodeHub.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    
    internal static class ViewEnginesConfig
    {
        internal static void RegisterViewEngines(ViewEngineCollection viewEnginesCollection)
        {
            viewEnginesCollection.Clear();
            viewEnginesCollection.Add(new RazorViewEngine());
        }
    }
}