using Escc.Umbraco.LinksManager.Services.Interfaces;
using Exceptionless;
using System;
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

                return PartialView("CheckInboundLinks", modelList);
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