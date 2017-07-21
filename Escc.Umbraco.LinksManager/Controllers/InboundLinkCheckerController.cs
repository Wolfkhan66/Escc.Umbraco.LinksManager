using Escc.Umbraco.LinksManager.Models.InboundLinkChecker;
using Escc.Umbraco.LinksManager.Services.Interfaces;
using Exceptionless;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace Escc.Umbraco.LinksManager.Controllers
{
    public class InboundLinkCheckerController : Controller
    {
        // GET: InboundLinkChecker
        private readonly IUmbracoService _umbracoService;

        public InboundLinkCheckerController(IUmbracoService umbracoService)
        {
            _umbracoService = umbracoService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FindInboundLinks(string url)
        {
            try
            {
                var modelList = _umbracoService.FindInboundLinks(url);

                if (modelList == null)
                {
                    TempData["MsgKey"] = "PageNotFound";

                    return PartialView("ToolsError");
                }
                if (modelList.PageId == 0)
                {
                    TempData["MsgKey"] = "PageNotFound";

                    return PartialView("ToolsError");
                }

                var siteUri = ConfigurationManager.AppSettings["SiteUri"];
                var editurl = string.Format("{0}#/content/content/edit/", siteUri);
                var viewurl = siteUri.Replace("/umbraco/", "");
                var redirectEditUri = ConfigurationManager.AppSettings["RedirectEdit"];
                var unpublishedStates = new List<string> { "[Parent unpublished]", "[Currently unpublished]", "[Not yet published]" };

                var model = new InboundLinkCheckerViewModel();
                model.PageName = modelList.PageName;
                modelList.PageId = modelList.PageId;

                model.LocalLinks.Table = new DataTable();
                model.LocalLinks.Table.Columns.Add("Page Name", typeof(string));
                model.LocalLinks.Table.Columns.Add("Fields", typeof(string));
                model.LocalLinks.Table.Columns.Add("Edit Link", typeof(HtmlString));
                model.LocalLinks.Table.Columns.Add("Published Link", typeof(HtmlString));

                foreach (var item in modelList.InboundLinksLocal)
                {
                    var EditLink = new HtmlString(string.Format("<a class=\"ui - button\" href=\"{0}{1}\" target=\"_blank\">edit page</a>", editurl, item.PageId));
                    var PublishedLinkString = "";
                    if (!unpublishedStates.Contains(item.PageUrl))
                    {
                        PublishedLinkString = string.Format("<a class=\"ui-button\" href=\"{0}{1}\" target=\"_blank\">view page</a>", viewurl, item.PageUrl);
                    }
                    else
                    {
                        PublishedLinkString = item.PageUrl;
                    }
                    var PublishedLinkHtmlString = new HtmlString(PublishedLinkString);
                    model.LocalLinks.Table.Rows.Add(item.PageName, string.Join(", ", item.FieldNames), EditLink, PublishedLinkHtmlString);
                }

                model.RedirectedLinks.Table = new DataTable();
                model.RedirectedLinks.Table.Columns.Add("Url", typeof(string));
                model.RedirectedLinks.Table.Columns.Add("Edit Link", typeof(HtmlString));
                model.RedirectedLinks.Table.Columns.Add("Redirect Type", typeof(string));
                model.RedirectedLinks.Table.Columns.Add("Comment", typeof(string));

                foreach (var item in modelList.InboundLinksRedirect)
                {
                    var EditLink = new HtmlString(string.Format("<a class=\"ui - button\" href=\"{0}{1}\" target=\"_blank\">edit page</a>", redirectEditUri, item.RedirectId));
                    var type = item.Type == 1 ? "Short Url" : "Moved Page";
                    model.RedirectedLinks.Table.Rows.Add(item.Pattern, EditLink, type, item.Comment);
                }

                model.ExternalLinks.Table = new DataTable();
                model.ExternalLinks.Table.Columns.Add("Referrer", typeof(string));
                model.ExternalLinks.Table.Columns.Add("Type", typeof(string));
                model.ExternalLinks.Table.Columns.Add("Link", typeof(HtmlString));

                foreach (var item in modelList.InboundLinksExternal)
                {
                    var Link = new HtmlString(string.Format("<a class=\"ui - button\" href=\"{0}\" target=\"_blank\">edit page</a>", item.Referrer));
                    model.ExternalLinks.Table.Rows.Add(item.Referrer, item.Type, Link);
                }

                return PartialView("CheckInboundLinks", model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                TempData["MsgKey"] = string.Format("ErrorOccurred");

                return PartialView("ToolsError");
            }
        }
    }
}