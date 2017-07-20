using Escc.Umbraco.LinksManager.Models.DataModels;

namespace Escc.Umbraco.LinksManager.Models.BrokenLinks
{
    public class BrokenLinksViewModel
    {
        public TableModel BrokenLinksTable { get; set; }
        public int ViewResultsFrom { get; set; }
        public BrokenLinksViewModel()
        {
            ViewResultsFrom = 1;
            BrokenLinksTable = new TableModel("BrokenLinksTable");
        }
    }
}