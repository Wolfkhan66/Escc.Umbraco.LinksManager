using System.Collections.Generic;
using Escc.Umbraco.LinksManager.Models;

namespace Escc.Umbraco.LinksManager.Services.Interfaces
{
    interface IRedirectsService
    {
        IList<RedirectModel> GetRedirectsByDestination(string destinationUrl);
    }
}
