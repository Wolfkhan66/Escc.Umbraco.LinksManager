using Escc.Umbraco.LinksManager.Models.BrokenLinks;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;

namespace Escc.Umbraco.LinksManager.Controllers
{
    public class BrokenLinkCheckerController : Controller
    {
        #region Global Variables
        public string _filePath = ConfigurationManager.AppSettings["InspyderBrokenLinks"];
        #endregion

        #region ActionResults
        public ActionResult Index()
        {
            var model = new BrokenLinksViewModel();
            model.TotalBroken = CountCSVRows("Link Errors");
            return View(model);
        }

        [Route("ViewAll", Name = "ViewAll")]
        public ActionResult ViewAll(string PagedResults = "Default", int ViewResultsFrom = 1, int TotalBroken = 0)
        {
            var model = new BrokenLinksViewModel();
            model.TotalBroken = TotalBroken;

            try
            {
                if (PagedResults == "Next")
                {
                    if (model.ViewResultsFrom < TotalBroken)
                    {
                        model.ViewResultsFrom = (model.ViewResultsFrom + 500);
                    }

                }
                else if (PagedResults == "Previous")
                {
                    if (model.ViewResultsFrom > 500)
                    {
                        model.ViewResultsFrom = (model.ViewResultsFrom - 500);
                    }
                }
                model.BrokenLinksTable.Table = PrepareViewAllDataTable(model.ViewResultsFrom);
            }
            catch (Exception error)
            {
                model.ErrorMessage = error.Message;
            }
            return View(model);
        }
        #endregion

        #region Helpers
        public void CheckFilePath()
        {
            // Check a file path was supplied
            if (string.IsNullOrEmpty(_filePath))
            {
                var err = new Exception("No File Path Supplied!");
                throw err;
            }

            // Check file is present & accessible
            if (!System.IO.File.Exists(_filePath))
            {
                var err = new Exception("No CSV file was found or it is not accessible!");
                throw err;
            }
        }

        public DataTable PrepareViewAllDataTable(int ViewResultsFrom)
        {
            var Results = ProcessCSVFile("Link Errors", ViewResultsFrom);

            var Table = new DataTable();
            Table.Columns.Add("Referrer", typeof(string));
            Table.Columns.Add("Broken Link", typeof(string));
            Table.Columns.Add("Error Type", typeof(string));

            foreach (var field in Results)
            {
                Table.Rows.Add(field[0], field[1], field[2]);
            }
            return Table;
        }

        public List<string[]> ProcessCSVFile(string fileType, int ViewResultsFrom)
        {
            CheckFilePath();
            var Results = new List<string[]>();
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
                } while (!d.StartsWith(fileType));

                // Read past the row with the column names
                csvParser.ReadLine();

                int lineNumber = 0;
                while (!csvParser.EndOfData)
                {
                    lineNumber++;
                    if (lineNumber >= (ViewResultsFrom + 500))
                    {
                        break;
                    }
                    // limit the amount of results to return
                    if (lineNumber >= ViewResultsFrom && lineNumber <= (ViewResultsFrom + 499))
                    {
                        // Read current line fields, pointer moves to the next line.
                        var fields = csvParser.ReadFields();

                        // empty row? Move to the next one
                        if (fields == null) continue;
                        Results.Add(fields);
                    }
                    else
                    {
                        // if we havent reached the results we need yet, then advance the pointer to the next line
                        csvParser.ReadLine();
                    }
                }
            }
            return Results;
        }

        public int CountCSVRows(string fileType)
        {
            CheckFilePath();
            int lineNumber = 0;
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
                } while (!d.StartsWith(fileType));

                // Read past the row with the column names
                csvParser.ReadLine();
                while (!csvParser.EndOfData)
                {
                    lineNumber++;
                    csvParser.ReadLine();
                }
            }
            return lineNumber;
        }
        #endregion

    }
}
