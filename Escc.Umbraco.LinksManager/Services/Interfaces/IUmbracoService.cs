using Escc.Umbraco.LinksManager.Models;

namespace Escc.Umbraco.LinksManager.Services.Interfaces
{
    public interface IUmbracoService
    {
        PageLinksModel FindInboundLinks(string url);
    }
}