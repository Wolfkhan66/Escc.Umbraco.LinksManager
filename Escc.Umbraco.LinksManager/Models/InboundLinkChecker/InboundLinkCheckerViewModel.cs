using Escc.Umbraco.LinksManager.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.Umbraco.LinksManager.Models.InboundLinkChecker
{
    public class InboundLinkCheckerViewModel
    {
        public TableModel LocalLinks { get; set; }
        public TableModel RedirectedLinks { get; set; }
        public TableModel ExternalLinks { get; set; }

        public string PageName { get; set; }
        public int PageID { get; set; }

        public InboundLinkCheckerViewModel()
        {
            LocalLinks = new TableModel("LocalLinksTable");
            RedirectedLinks = new TableModel("RedirectedLinksTable");
            ExternalLinks = new TableModel("ExternalLinksTable");
        }
    }
}