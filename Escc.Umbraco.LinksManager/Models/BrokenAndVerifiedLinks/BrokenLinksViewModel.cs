using Escc.Umbraco.LinksManager.Models.DataModels;

namespace Escc.Umbraco.LinksManager.Models.BrokenAndVerifiedLinks
{
    public class BrokenLinksViewModel
    {
        public TableModel BrokenLinksTable { get; set; }
        public int TotalBroken { get; set; }
        public int ShowResultsFrom { get; set; }
        public string ErrorMessage { get; set; }
        public string Query { get; set; }
        public bool Search { get; set; }

        public BrokenLinksViewModel()
        {
            Search = false;
            ShowResultsFrom = 0;
            BrokenLinksTable = new TableModel("BrokenLinksTable");
        }

    }
}