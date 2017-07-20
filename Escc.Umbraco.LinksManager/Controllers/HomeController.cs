using System.Web.Mvc;

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