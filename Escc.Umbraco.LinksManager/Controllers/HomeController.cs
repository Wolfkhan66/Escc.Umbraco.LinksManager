using System;
using System.Web.Mvc;
using Escc.Umbraco.LinksManager.Services.Interfaces;
using Exceptionless;

namespace Escc.Umbraco.LinksManager.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InboundLinkChecker()
        {
            return View("InboundLinkChecker/Index.cshtml");
        }


    }
}