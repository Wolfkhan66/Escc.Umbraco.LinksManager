using System;

namespace Escc.Umbraco.LinksManager.Utility
{
    public static class Functions
    {
        public static string GetAppPath(string appPath)
        {
            if (String.IsNullOrEmpty(appPath))
            {
                appPath = "/.";
            }
            else if (appPath.Length == 1)
            {
                appPath += ".";
            }
            else if (!appPath.EndsWith("/"))
            {
                appPath += "/";
            }

            return appPath;
        }
    }
}