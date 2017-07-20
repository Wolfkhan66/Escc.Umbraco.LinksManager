using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using Escc.Umbraco.LinksManager.Models.InboundLinkChecker;
using Escc.Umbraco.LinksManager.Services.Interfaces;

namespace Escc.Umbraco.LinksManager.Services
{
    public class UmbracoService : IUmbracoService
    {
        private readonly HttpClient _client;

        public UmbracoService()
        {
            var siteUri = ConfigurationManager.AppSettings["SiteUri"];

            siteUri = string.Format("{0}Api/UmbracoUserApi/", siteUri);
            var handler = new HttpClientHandler
            {
                Credentials =
                    new NetworkCredential(ConfigurationManager.AppSettings["apiuser"],
                        ConfigurationManager.AppSettings["apikey"])
            };

            _client = new HttpClient(handler) { BaseAddress = new Uri(siteUri) };
        }

        /// <summary>
        /// Sends a Get request to a web API with the given URL
        /// </summary>
        /// <param name="uriPath">URL of the web API method</param>
        /// <returns> Response code - Ok or BadGateway</returns>
        private HttpResponseMessage GetMessage(string uriPath)
        {
            var response = _client.GetAsync(uriPath).Result;
            return response;
        }

        /// <summary>
        /// Get a list of all inbound links to the provided Url
        /// </summary>
        /// <param name="url">page Url</param>
        /// <returns>List of links into the provided url</returns>
        public PageLinksModel FindInboundLinks(string url)
        {
            // Search Umbraco for internal links
            var response = GetMessage(string.Format("GetPageInboundLinks?url={0}", url));

            if (!response.IsSuccessStatusCode) return null;
            var model = response.Content.ReadAsAsync<PageLinksModel>().Result;
            // Search the Redirects Database
            if (!string.IsNullOrEmpty(model.PageUrl))
            {
                GetPageInboundLinks_Redirects(model);
            }

            // Search the Inspyder extract
            GetExternalLinks_Inspyder(model, url);

            return model;
        }

        private void GetPageInboundLinks_Redirects(PageLinksModel model)
        {
            var rd = new RedirectsService();

            var url = model.PageUrl;

            // List<RedirectModel> links
            model.InboundLinksRedirect.AddRange(rd.GetRedirectsByDestination(url));
        }

        private void GetExternalLinks_Inspyder(PageLinksModel model, string url)
        {
            // Get file path from config
            var inspyderCsvFilePath = ConfigurationManager.AppSettings["InspyderCsvFile"];
            if (string.IsNullOrEmpty(inspyderCsvFilePath)) return;

            var el = new CsvFileService(inspyderCsvFilePath);

            var res = el.GetLinksByDestination(url);

            model.InboundLinksExternal.AddRange(res);
        }
    }
}