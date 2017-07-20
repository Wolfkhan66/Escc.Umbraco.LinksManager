using Escc.Umbraco.LinksManager.Models.BrokenLinks;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Data;
using System.Web.Mvc;

namespace Escc.Umbraco.LinksManager.Controllers
{
    public class BrokenLinkCheckerController : Controller
    {
        public string _filePath = ConfigurationManager.AppSettings["InspyderBrokenLinks"];
        // GET: BrokenLinkChecker
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll(BrokenLinksViewModel model = null)
        {
            if (model == null)
            {
                model = new BrokenLinksViewModel();
            }
            model.BrokenLinksTable.Table = PrepareViewAllDataTable(model.ViewResultsFrom);

            return View(model);
        }

        [Route("Next", Name = "Next")]
        public ActionResult Next(int ViewResultsFrom)
        {
            var model = new BrokenLinksViewModel();
            model.ViewResultsFrom = (ViewResultsFrom + 1000);
            ViewAll(model);
            return View("ViewAll" ,model);
        }

        [Route("Previous", Name = "Previous")]
        public ActionResult Previous(int ViewResultsFrom)
        {
            var model = new BrokenLinksViewModel();
            if (ViewResultsFrom > 1000)
            {
                model.ViewResultsFrom = (ViewResultsFrom - 1000);
            }
            ViewAll(model);
            return View("ViewAll", model);
        }

        public DataTable PrepareViewAllDataTable(int ViewResultsFrom)
        {
            var Table = new DataTable();
            Table.Columns.Add("Referrer", typeof(string));
            Table.Columns.Add("Broken Link", typeof(string));
            Table.Columns.Add("Error Type", typeof(string));

            // Check a file path was supplied
            if (string.IsNullOrEmpty(_filePath))
            {
                // Throw Exception
            }

            // Check file is present & accessible
            if (!System.IO.File.Exists(_filePath))
            {
                // Throw Exception
            }

            using (var csvParser = new TextFieldParser(_filePath))
            {
                csvParser.CommentTokens = new[] { "#" };
                csvParser.SetDelimiters(",");
                csvParser.HasFieldsEnclosedInQuotes = true;

                string d;
                // Read until we find the Report Title line (Broken Links)
                do
                {
                    d = csvParser.ReadLine();
                    if (d == null)
                    {
                        // Throw Exception
                    };
                } while (!d.StartsWith("Link Errors"));

                // Read past the row with the column names
                csvParser.ReadLine();

                int lineNumber = 0;
                while (!csvParser.EndOfData)
                {
                    lineNumber++;
                    // break the loop once we have 1000 results to display
                    if (lineNumber >= (ViewResultsFrom + 1000))
                    {
                        break;
                    }
                    // limit the amount of results to return
                    if (lineNumber >= ViewResultsFrom && lineNumber <= (ViewResultsFrom + 999) )
                    {
                        // Read current line fields, pointer moves to the next line.
                        var fields = csvParser.ReadFields();

                        // empty row? Move to the next one
                        if (fields == null) continue;
                        Table.Rows.Add(fields[0], fields[1], fields[2]);
                    }
                }
            }
            return Table;
        }



    }
}
