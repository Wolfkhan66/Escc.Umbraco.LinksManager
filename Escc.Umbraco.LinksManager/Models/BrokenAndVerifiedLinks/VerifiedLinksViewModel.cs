using Escc.Umbraco.LinksManager.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.Umbraco.LinksManager.Models.BrokenAndVerifiedLinks
{
    public class VerifiedLinksViewModel
    {    
        public TableModel VerifiedLinksTable { get; set; }
        public int TotalVerified { get; set; }
        public int ShowResultsFrom { get; set; }
        public string ErrorMessage { get; set; }
        public string Query { get; set; }
        public bool Search { get; set; }

        public VerifiedLinksViewModel()
        {
            Search = false;
            ShowResultsFrom = 0;
            VerifiedLinksTable = new TableModel("VerifiedLinksTable");
        }

    }
}