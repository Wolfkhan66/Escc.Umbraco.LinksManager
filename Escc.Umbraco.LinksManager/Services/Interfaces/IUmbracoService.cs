using Escc.Umbraco.LinksManager.Models.InboundLinkChecker;

namespace Escc.Umbraco.LinksManager.Services.Interfaces
{
    public interface IUmbracoService
    {
        PageLinksModel FindInboundLinks(string url);
    }
}